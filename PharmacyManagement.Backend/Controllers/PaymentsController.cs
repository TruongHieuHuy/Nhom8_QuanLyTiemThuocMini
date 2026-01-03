using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using PharmacyManagement.DTOs;
using PharmacyManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;
        private readonly PharmacyContext _context;

        public PaymentsController(IOrderService orderService, IVnPayService vnPayService, PharmacyContext context)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
            _context = context;
        }

        // 1) Tạo đơn PendingPayment + trả về URL VNPay để redirect
        [HttpPost("vnpay/create")]
        public async Task<ActionResult<VnPayCreatePaymentResponseDTO>> CreateVnPayPayment([FromBody] CreateOrderDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Tạo order ở trạng thái PendingPayment, CHƯA trừ kho
                var result = await _orderService.CreateOrderAsync(
                    dto,
                    deductStockNow: false,
                    forcedPaymentMethod: "VNPay",
                    forcedStatus: "PendingPayment"
                );


                // Payment history: create pending transaction
                _context.PaymentTransactions.Add(new PaymentTransaction
                {
                    OrderId = result.OrderId,
                    OrderCode = result.OrderCode,
                    Provider = "VNPay",
                    PaymentMethod = "VNPay",
                    Amount = result.Total,
                    Currency = "VND",
                    Status = "Pending",
                    TxnRef = result.OrderCode,
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                // VNPay dùng đơn vị *100
                var amountTimes100 = (long)decimal.Round(result.Total * 100, 0);

                var ip = HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                var paymentUrl = _vnPayService.CreatePaymentUrl(
                    ip,
                    txnRef: result.OrderCode,
                    amountVndTimes100: amountTimes100,
                    orderInfo: $"Thanh toan don {result.OrderCode}"
                );

                return Ok(new VnPayCreatePaymentResponseDTO
                {
                    PaymentUrl = paymentUrl,
                    OrderId = result.OrderId,
                    OrderCode = result.OrderCode,
                    Amount = result.Total
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 2) ReturnUrl (người dùng quay về từ VNPay)
        // VNPay sẽ gọi returnUrl với query params (vnp_*)
        [HttpGet("vnpay/return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var q = Request.Query;

            // build dict
            var dict = q.Keys.ToDictionary(k => k, k => q[k].ToString());
            dict.TryGetValue("vnp_SecureHash", out var secureHash);

            var isValid = _vnPayService.ValidateSignature(dict, secureHash);

            dict.TryGetValue("vnp_TxnRef", out var txnRef);
            dict.TryGetValue("vnp_ResponseCode", out var responseCode);
            dict.TryGetValue("vnp_TransactionStatus", out var transStatus);

            var settings = _vnPayService.GetSettings();
            var fe = string.IsNullOrWhiteSpace(settings.FrontendReturnUrl) ? "http://localhost:3000/orders" : settings.FrontendReturnUrl;

            if (!isValid)
            {
                return Redirect($"{fe}?vnpay=1&status=invalid_signature&orderCode={txnRef}");
            }

            var ok = responseCode == "00" && (string.IsNullOrWhiteSpace(transStatus) || transStatus == "00");

            var rawJson = System.Text.Json.JsonSerializer.Serialize(dict);

            // update latest pending transaction for this order
            var tx = await _context.PaymentTransactions
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync(t => t.OrderCode == txnRef && t.Status == "Pending");
            if (tx != null)
            {
                tx.ResponseCode = responseCode;
                tx.TransactionNo = dict.ContainsKey("vnp_TransactionNo") ? dict["vnp_TransactionNo"] : null;
                tx.BankCode = dict.ContainsKey("vnp_BankCode") ? dict["vnp_BankCode"] : null;
                tx.PayDate = dict.ContainsKey("vnp_PayDate") ? dict["vnp_PayDate"] : null;
                tx.RawData = rawJson;
                tx.UpdatedAt = DateTime.UtcNow;
            }

            if (ok)
            {
                var paid = await _orderService.MarkOrderPaidAsync(txnRef);

                if (tx != null && paid)
                {
                    tx.Status = "Success";
                    tx.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                if (tx != null && paid)
                {
                    tx.Status = "Success";
                    tx.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                return Redirect($"{fe}?vnpay=1&status={(paid ? "success" : "failed")}&orderCode={txnRef}");
            }
            else
            {
                await _orderService.MarkOrderFailedAsync(txnRef, $"VNPay return fail: {responseCode}/{transStatus}");

            if (tx != null)
            {
                tx.Status = "Failed";
                tx.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
                return Redirect($"{fe}?vnpay=1&status=failed&orderCode={txnRef}");
            }
        }

        // 3) IPN (server-to-server)
        // Sandbox thật có thể gọi endpoint này. Khi deploy thực tế, nên mở public URL.
        [HttpGet("vnpay/ipn")]
        public async Task<IActionResult> VnPayIpn()
        {
            var q = Request.Query;
            var dict = q.Keys.ToDictionary(k => k, k => q[k].ToString());
            dict.TryGetValue("vnp_SecureHash", out var secureHash);

            if (!_vnPayService.ValidateSignature(dict, secureHash))
            {
                return Ok(new { RspCode = "97", Message = "Invalid Signature" });
            }

            dict.TryGetValue("vnp_TxnRef", out var txnRef);
            dict.TryGetValue("vnp_ResponseCode", out var responseCode);
            dict.TryGetValue("vnp_TransactionStatus", out var transStatus);

            var ok = responseCode == "00" && (string.IsNullOrWhiteSpace(transStatus) || transStatus == "00");

            var rawJson = System.Text.Json.JsonSerializer.Serialize(dict);

            // update latest pending transaction for this order
            var tx = await _context.PaymentTransactions
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync(t => t.OrderCode == txnRef && t.Status == "Pending");
            if (tx != null)
            {
                tx.ResponseCode = responseCode;
                tx.TransactionNo = dict.ContainsKey("vnp_TransactionNo") ? dict["vnp_TransactionNo"] : null;
                tx.BankCode = dict.ContainsKey("vnp_BankCode") ? dict["vnp_BankCode"] : null;
                tx.PayDate = dict.ContainsKey("vnp_PayDate") ? dict["vnp_PayDate"] : null;
                tx.RawData = rawJson;
                tx.UpdatedAt = DateTime.UtcNow;
            }

            if (ok)
            {
                var paid = await _orderService.MarkOrderPaidAsync(txnRef);
                return Ok(new { RspCode = paid ? "00" : "01", Message = paid ? "Confirm Success" : "Order not found / not pending" });
            }

            await _orderService.MarkOrderFailedAsync(txnRef, $"VNPay IPN fail: {responseCode}/{transStatus}");
            if (tx != null)
            {
                tx.Status = "Failed";
                tx.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return Ok(new { RspCode = "00", Message = "Confirm Success" });
        }

        // 3) Payment history (for UI)
        // GET: /api/Payments/history?orderCode=...&status=...&provider=...&from=2026-01-01&to=2026-01-31&page=1&pageSize=20
        [HttpGet("history")]
        public async Task<ActionResult<PaymentHistoryResponseDTO>> GetPaymentHistory(
            [FromQuery] string orderCode = null,
            [FromQuery] string status = null,
            [FromQuery] string provider = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20
        )
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;
            if (pageSize > 200) pageSize = 200;

            var query = _context.PaymentTransactions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(orderCode))
                query = query.Where(t => t.OrderCode.Contains(orderCode));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status == status);

            if (!string.IsNullOrWhiteSpace(provider))
                query = query.Where(t => t.Provider == provider || t.PaymentMethod == provider);

            if (from.HasValue)
                query = query.Where(t => t.CreatedAt >= from.Value.ToUniversalTime());

            if (to.HasValue)
                query = query.Where(t => t.CreatedAt <= to.Value.ToUniversalTime());

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new PaymentHistoryItemDTO
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    OrderCode = t.OrderCode,
                    Provider = t.Provider,
                    PaymentMethod = t.PaymentMethod,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Status = t.Status,
                    TxnRef = t.TxnRef,
                    TransactionNo = t.TransactionNo,
                    ResponseCode = t.ResponseCode,
                    BankCode = t.BankCode,
                    PayDate = t.PayDate,
                    RawData = t.RawData,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return Ok(new PaymentHistoryResponseDTO
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = items
            });
        }

    }
}
