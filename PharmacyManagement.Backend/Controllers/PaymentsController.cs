using Microsoft.AspNetCore.Mvc;
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

        public PaymentsController(IOrderService orderService, IVnPayService vnPayService)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
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

            if (ok)
            {
                var paid = await _orderService.MarkOrderPaidAsync(txnRef);
                return Redirect($"{fe}?vnpay=1&status={(paid ? "success" : "failed")}&orderCode={txnRef}");
            }
            else
            {
                await _orderService.MarkOrderFailedAsync(txnRef, $"VNPay return fail: {responseCode}/{transStatus}");
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

            if (ok)
            {
                var paid = await _orderService.MarkOrderPaidAsync(txnRef);
                return Ok(new { RspCode = paid ? "00" : "01", Message = paid ? "Confirm Success" : "Order not found / not pending" });
            }

            await _orderService.MarkOrderFailedAsync(txnRef, $"VNPay IPN fail: {responseCode}/{transStatus}");
            return Ok(new { RspCode = "00", Message = "Confirm Success" });
        }
    }
}
