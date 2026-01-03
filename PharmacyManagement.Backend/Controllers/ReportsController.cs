using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public ReportsController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/reports/top-medicines/10
        [HttpGet("top-medicines/{count}")]
        public async Task<IActionResult> GetTopMedicines(int count = 10)
        {
            var topMedicines = await _context.OrderDetails
                .Where(od => od.Order.OrderStatus == "Completed") // Chỉ tính đơn đã hoàn thành
                .GroupBy(od => od.MedicineId)
                .Select(g => new
                {
                    MedicineId = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .Join(
                    _context.Medicines,
                    stat => stat.MedicineId,
                    med => med.Id,
                    (stat, med) => new
                    {
                        id = med.Id,
                        name = med.Name,
                        price = med.Price,
                        currentStock = med.CurrentStock,
                        totalSold = stat.TotalSold,
                        unit = med.Unit
                    }
                )
                .ToListAsync();

            return Ok(topMedicines);
        }

        // GET: api/reports/top-customers/10
        [HttpGet("top-customers/{count}")]
        public async Task<IActionResult> GetTopCustomers(int count = 10)
        {
            var topCustomers = await _context.Orders
                .Where(o => o.CustomerId != null && o.OrderStatus == "Completed") // Chỉ tính khách có tài khoản và đơn hoàn thành
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalSpending = g.Sum(o => o.Total)
                })
                .OrderByDescending(x => x.TotalSpending)
                .Take(count)
                .Join(
                    _context.Customers,
                    stat => stat.CustomerId,
                    cust => cust.Id,
                    (stat, cust) => new
                    {
                        id = cust.Id,
                        name = cust.Name,
                        phone = cust.PhoneNumber,
                        email = cust.Email,
                        totalSpending = stat.TotalSpending
                    }
                )
                .ToListAsync();

            return Ok(topCustomers);
        }

        // GET: api/reports/out-of-stock
        [HttpGet("out-of-stock")]
        public async Task<IActionResult> GetOutOfStock()
        {
            var outOfStock = await _context.Medicines
                .Where(m => m.CurrentStock == 0 && m.IsActive)
                .Select(m => new
                {
                    id = m.Id,
                    name = m.Name,
                    minStockLevel = m.MinStockLevel,
                    currentStock = m.CurrentStock,
                    unit = m.Unit,
                    barcode = m.Barcode
                })
                .ToListAsync();

            return Ok(outOfStock);
        }

        // BONUS: GET: api/reports/revenue/date/{date}
        [HttpGet("revenue/date/{date}")]
        public async Task<IActionResult> GetRevenueByDate(string date)
        {
            if (!System.DateTime.TryParse(date, out var targetDate))
                return BadRequest(new { message = "Ngày không hợp lệ" });

            var revenue = await _context.Orders
                .Where(o => o.OrderDate.Date == targetDate.Date && o.OrderStatus == "Completed")
                .SumAsync(o => o.Total);

            return Ok(new { date = targetDate.ToString("yyyy-MM-dd"), revenue });
        }

        // BONUS: GET: api/reports/revenue/range?startDate=...&endDate=...
        [HttpGet("revenue/range")]
        public async Task<IActionResult> GetRevenueByRange([FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!System.DateTime.TryParse(startDate, out var start) || !System.DateTime.TryParse(endDate, out var end))
                return BadRequest(new { message = "Ngày không hợp lệ" });

            var revenue = await _context.Orders
                .Where(o => o.OrderDate.Date >= start.Date && o.OrderDate.Date <= end.Date && o.OrderStatus == "Completed")
                .SumAsync(o => o.Total);

            var ordersCount = await _context.Orders
                .Where(o => o.OrderDate.Date >= start.Date && o.OrderDate.Date <= end.Date && o.OrderStatus == "Completed")
                .CountAsync();

            return Ok(new
            {
                startDate = start.ToString("yyyy-MM-dd"),
                endDate = end.ToString("yyyy-MM-dd"),
                revenue,
                ordersCount
            });
        }

        // BONUS: GET: api/reports/orders-count/{date}
        [HttpGet("orders-count/{date}")]
        public async Task<IActionResult> GetOrdersCount(string date)
        {
            if (!System.DateTime.TryParse(date, out var targetDate))
                return BadRequest(new { message = "Ngày không hợp lệ" });

            var count = await _context.Orders
                .Where(o => o.OrderDate.Date == targetDate.Date)
                .CountAsync();

            return Ok(new { date = targetDate.ToString("yyyy-MM-dd"), count });
        }
    }
}
