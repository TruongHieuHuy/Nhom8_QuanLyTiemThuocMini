using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResultDTO> CreateOrderAsync(CreateOrderDTO dto, bool deductStockNow = true, string forcedPaymentMethod = null, string forcedStatus = null);
        Task<bool> MarkOrderPaidAsync(string orderCode);
        Task<bool> MarkOrderFailedAsync(string orderCode, string failNote = null);
    }

    public class OrderService : IOrderService
    {
        private readonly PharmacyContext _context;

        public OrderService(PharmacyContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResultDTO> CreateOrderAsync(CreateOrderDTO dto, bool deductStockNow = true, string forcedPaymentMethod = null, string forcedStatus = null)
        {
            if (dto == null || dto.OrderDetails == null || dto.OrderDetails.Count == 0)
                throw new ArgumentException("OrderDetails is required");

            var now = DateTime.Now;
            var orderCode = $"HD{now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";

            var order = new Order
            {
                OrderCode = orderCode,
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId,
                OrderDate = now,
                Discount = dto.Discount,
                Tax = dto.Tax,
                Notes = dto.Notes,
                CreatedDate = now,
                PaymentMethod = forcedPaymentMethod ?? dto.PaymentMethod ?? "Cash",
                OrderStatus = forcedStatus ?? "Completed"
            };

            decimal subTotal = 0;

            foreach (var item in dto.OrderDetails)
            {
                if (item.Quantity <= 0)
                    throw new ArgumentException("Quantity must be > 0");

                var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == item.MedicineId && m.IsActive);
                if (medicine == null)
                    throw new InvalidOperationException($"Medicine not found: {item.MedicineId}");

                if (deductStockNow && medicine.CurrentStock < item.Quantity)
                    throw new InvalidOperationException($"Không đủ tồn kho cho thuốc: {medicine.Name}");

                var unitPrice = medicine.Price;
                var lineTotal = unitPrice * item.Quantity;

                order.OrderDetails.Add(new OrderDetail
                {
                    MedicineId = medicine.Id,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = lineTotal
                });

                subTotal += lineTotal;

                if (deductStockNow)
                {
                    await ExportStockAsync(medicine, item.Quantity, orderCode);
                }
            }

            order.SubTotal = subTotal;

            // Total = SubTotal - Discount + Tax (không âm)
            var total = subTotal - dto.Discount + dto.Tax;
            order.Total = total < 0 ? 0 : total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var pm = (order.PaymentMethod ?? "").Trim();

            if (!pm.Equals("VNPay", StringComparison.OrdinalIgnoreCase)
                && order.OrderStatus == "Completed")
            {
                var tx = new PaymentTransaction
                {
                    OrderId = order.Id,
                    OrderCode = order.OrderCode,

                    Provider = pm,          // "Cash" hoặc "Banking"

                    Amount = order.Total,
                    Status = "Success",

                    ResponseCode = "00",
                    TransactionNo = null,
                    BankCode = null,

                    // ✅ PayDate đang là string => convert DateTime -> string
                    PayDate = now.ToString("yyyyMMddHHmmss") // hoặc "O" nếu bạn thích ISO
                };

                _context.PaymentTransactions.Add(tx);
                await _context.SaveChangesAsync();
            }

            return new CreateOrderResultDTO
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                Total = order.Total,
                OrderStatus = order.OrderStatus
            };
        }

        public async Task<bool> MarkOrderPaidAsync(string orderCode)
        {
            if (string.IsNullOrWhiteSpace(orderCode))
                return false;

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

            if (order == null)
                return false;

            if (order.OrderStatus == "Completed")
                return true;

            if (order.OrderStatus != "PendingPayment")
            {
                // Chỉ cho phép hoàn tất đơn đang chờ thanh toán
                return false;
            }

            foreach (var detail in order.OrderDetails)
            {
                var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == detail.MedicineId && m.IsActive);
                if (medicine == null)
                    return false;

                if (medicine.CurrentStock < detail.Quantity)
                {
                    order.OrderStatus = "Failed";
                    order.Notes = (order.Notes ?? "") + " | VNPay: Không đủ tồn kho khi xác nhận thanh toán";
                    await _context.SaveChangesAsync();
                    return false;
                }

                await ExportStockAsync(medicine, detail.Quantity, order.OrderCode);
            }

            order.OrderStatus = "Completed";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkOrderFailedAsync(string orderCode, string failNote = null)
        {
            if (string.IsNullOrWhiteSpace(orderCode))
                return false;

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
                return false;

            if (order.OrderStatus == "Completed")
                return false;

            order.OrderStatus = "Failed";
            if (!string.IsNullOrWhiteSpace(failNote))
            {
                order.Notes = string.IsNullOrWhiteSpace(order.Notes)
                    ? failNote
                    : (order.Notes + " | " + failNote);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ExportStockAsync(Medicine medicine, int qty, string orderCode)
        {
            var before = medicine.CurrentStock;
            medicine.CurrentStock -= qty;

            _context.InventoryHistories.Add(new InventoryHistory
            {
                MedicineId = medicine.Id,
                TransactionType = "Export",
                Quantity = qty,
                StockBefore = before,
                StockAfter = medicine.CurrentStock,
                Reason = "Sale",
                Notes = $"Order {orderCode}",
                CreatedDate = DateTime.Now
            });

            _context.Medicines.Update(medicine);

            // Lưu từng lần trừ để tránh race condition trong môi trường demo
            await _context.SaveChangesAsync();
        }
    }
}
