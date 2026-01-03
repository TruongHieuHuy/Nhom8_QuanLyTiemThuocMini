using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public DashboardController(PharmacyContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] string period = "day", [FromQuery] string startDateStr = null, [FromQuery] string endDateStr = null)
        {
            DateTime now = DateTime.Now;
            DateTime startDate, endDate, previousStartDate, previousEndDate;

            // Xác định khoảng thời gian dựa trên period
            switch (period.ToLower())
            {
                case "custom":
                    // Xử lý custom date range
                    if (!string.IsNullOrEmpty(startDateStr) && !string.IsNullOrEmpty(endDateStr))
                    {
                        if (!DateTime.TryParse(startDateStr, out startDate) || !DateTime.TryParse(endDateStr, out endDate))
                        {
                            return BadRequest("Invalid date format");
                        }
                        
                        // Đảm bảo endDate là cuối ngày
                        endDate = endDate.Date.AddDays(1);
                        startDate = startDate.Date;
                        
                        // Tính khoảng thời gian để so sánh với kỳ trước
                        var duration = endDate - startDate;
                        previousEndDate = startDate;
                        previousStartDate = startDate.AddDays(-duration.TotalDays);
                    }
                    else
                    {
                        return BadRequest("startDate and endDate are required for custom period");
                    }
                    break;

                case "week":
                    // Tính số ngày từ thứ 2 (Monday = 1, Sunday = 0 -> cần chuyển Sunday thành 7)
                    int daysFromMonday = (int)now.DayOfWeek == 0 ? 6 : (int)now.DayOfWeek - 1;
                    startDate = now.Date.AddDays(-daysFromMonday); // Đầu tuần (T2)
                    endDate = startDate.AddDays(7); // Hết tuần (CN)
                    previousStartDate = startDate.AddDays(-7);
                    previousEndDate = startDate;
                    break;

                case "month":
                    startDate = new DateTime(now.Year, now.Month, 1); // Ngày 1 của tháng
                    endDate = startDate.AddMonths(1); // Ngày 1 của tháng sau
                    previousStartDate = startDate.AddMonths(-1);
                    previousEndDate = startDate;
                    break;

                case "year":
                    startDate = new DateTime(now.Year, 1, 1); // Đầu năm nay
                    endDate = now.Date.AddDays(1); // Đến hết hôm nay (không phải hết năm)
                    previousStartDate = new DateTime(now.Year - 1, 1, 1); // Đầu năm trước
                    previousEndDate = new DateTime(now.Year - 1, now.Month, now.Day).AddDays(1); // Cùng ngày năm trước
                    break;

                default: // "day"
                    startDate = now.Date;
                    endDate = startDate.AddDays(1);
                    previousStartDate = startDate.AddDays(-1);
                    previousEndDate = startDate;
                    break;
            }

            // 1. Thuốc sắp hết (luôn lấy tổng, không theo period)
            var lowStock = await _context.Medicines
                .CountAsync(m => m.CurrentStock <= m.MinStockLevel && m.IsActive);

            // 2. Doanh thu trong kỳ hiện tại và kỳ trước
            var currentRevenue = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.OrderStatus == "Completed")
                .SumAsync(o => (decimal?)o.Total) ?? 0;

            var previousRevenue = await _context.Orders
                .Where(o => o.OrderDate >= previousStartDate && o.OrderDate < previousEndDate && o.OrderStatus == "Completed")
                .SumAsync(o => (decimal?)o.Total) ?? 0;

            var revenueGrowth = previousRevenue > 0 
                ? Math.Round((currentRevenue - previousRevenue) / previousRevenue * 100, 1) 
                : 0;

            // 3. Đơn hàng trong kỳ hiện tại và kỳ trước
            var currentOrders = await _context.Orders
                .CountAsync(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.OrderStatus == "Completed");

            var previousOrders = await _context.Orders
                .CountAsync(o => o.OrderDate >= previousStartDate && o.OrderDate < previousEndDate && o.OrderStatus == "Completed");

            var ordersGrowth = previousOrders > 0 
                ? Math.Round((double)(currentOrders - previousOrders) / previousOrders * 100, 1) 
                : 0;

            // 4. Khách hàng trong kỳ hiện tại và kỳ trước (khách đã mua hàng)
            var currentCustomers = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.OrderStatus == "Completed")
                .Select(o => o.CustomerId)
                .Distinct()
                .CountAsync();

            var previousCustomers = await _context.Orders
                .Where(o => o.OrderDate >= previousStartDate && o.OrderDate < previousEndDate && o.OrderStatus == "Completed")
                .Select(o => o.CustomerId)
                .Distinct()
                .CountAsync();

            var customersGrowth = previousCustomers > 0 
                ? Math.Round((double)(currentCustomers - previousCustomers) / previousCustomers * 100, 1) 
                : 0;

            return Ok(new
            {
                lowStock,
                totalRevenue = currentRevenue,
                revenueGrowth,
                totalOrders = currentOrders,
                ordersGrowth,
                totalCustomers = currentCustomers,
                customersGrowth,
                period,
                startDate = startDate.ToString("yyyy-MM-dd"),
                endDate = endDate.ToString("yyyy-MM-dd")
            });
        }
    }
}