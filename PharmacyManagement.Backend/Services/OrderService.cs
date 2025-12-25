using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(System.DateTime startDate, System.DateTime endDate);
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createDto);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> DeleteOrderAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public OrderService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.ToList()
            );
            return orders.Select(o => MapToDTO(o)).ToList();
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            var order = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.FirstOrDefault(o => o.Id == id)
            );
            return order != null ? MapToDTO(order) : null;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.Where(o => o.CustomerId == customerId).ToList()
            );
            return orders.Select(o => MapToDTO(o)).ToList();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(System.DateTime startDate, System.DateTime endDate)
        {
            var orders = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList()
            );
            return orders.Select(o => MapToDTO(o)).ToList();
        }

        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createDto)
        {
            var order = new Order
            {
                OrderCode = GenerateOrderCode(),
                CustomerId = createDto.CustomerId,
                EmployeeId = createDto.EmployeeId,
                OrderDate = System.DateTime.Now,
                SubTotal = createDto.OrderDetails.Sum(od => od.UnitPrice * od.Quantity),
                Discount = createDto.Discount,
                Tax = createDto.Tax,
                Total = 0,
                PaymentMethod = createDto.PaymentMethod,
                OrderStatus = "Pending",
                Notes = createDto.Notes,
                CreatedDate = System.DateTime.Now
            };

            order.Total = order.SubTotal - order.Discount + order.Tax;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order details
            foreach (var detail in createDto.OrderDetails)
            {
                var medicine = _context.Medicines.FirstOrDefault(m => m.Id == detail.MedicineId);
                if (medicine != null)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        MedicineId = detail.MedicineId,
                        Quantity = detail.Quantity,
                        UnitPrice = medicine.Price,
                        TotalPrice = medicine.Price * detail.Quantity
                    };

                    _context.OrderDetails.Add(orderDetail);

                    // Update medicine stock
                    medicine.CurrentStock -= detail.Quantity;
                    _context.Medicines.Update(medicine);

                    // Add inventory history
                    var inventoryHistory = new InventoryHistory
                    {
                        MedicineId = medicine.Id,
                        TransactionType = "Export",
                        Quantity = detail.Quantity,
                        StockBefore = medicine.CurrentStock + detail.Quantity,
                        StockAfter = medicine.CurrentStock,
                        Reason = "Sale",
                        CreatedDate = System.DateTime.Now
                    };
                    _context.InventoryHistories.Add(inventoryHistory);
                }
            }

            // Update customer total spending
            var customer = _context.Customers.FirstOrDefault(c => c.Id == order.CustomerId);
            if (customer != null)
            {
                customer.TotalSpending += order.Total;
                _context.Customers.Update(customer);
            }

            await _context.SaveChangesAsync();
            return MapToDTO(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.FirstOrDefault(o => o.Id == orderId)
            );

            if (order == null)
                return false;

            order.OrderStatus = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await System.Threading.Tasks.Task.Run(() =>
                _context.Orders.FirstOrDefault(o => o.Id == id)
            );

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        private OrderDTO MapToDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name,
                EmployeeId = order.EmployeeId,
                EmployeeName = order.Employee?.FullName,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                Tax = order.Tax,
                Total = order.Total,
                PaymentMethod = order.PaymentMethod,
                OrderStatus = order.OrderStatus,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDTO
                {
                    Id = od.Id,
                    MedicineId = od.MedicineId,
                    MedicineName = od.Medicine?.Name,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    TotalPrice = od.TotalPrice
                }).ToList()
            };
        }

        private string GenerateOrderCode()
        {
            return "ORD-" + System.DateTime.Now.Ticks.ToString().Substring(0, 10);
        }
    }
}
