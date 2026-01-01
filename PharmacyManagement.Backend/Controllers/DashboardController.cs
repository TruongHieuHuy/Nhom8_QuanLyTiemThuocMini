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
        public async Task<IActionResult> GetStats()
        {
            // 1. Đếm tổng thuốc
            var totalMedicines = await _context.Medicines.CountAsync(m => m.IsActive);

            // 2. Đếm tổng khách
            var totalCustomers = await _context.Customers.CountAsync(c => c.IsActive);

            // 3. Đếm thuốc sắp hết (Tồn kho <= Định mức)
            var lowStock = await _context.Medicines
                .CountAsync(m => m.CurrentStock <= m.MinStockLevel && m.IsActive);

            // 4. Tính tổng doanh thu & Đơn hàng (Lấy trực tiếp từ SQL)
            // Lưu ý: Nếu bảng Order trống thì Sum sẽ trả về 0
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.Total);

            return Ok(new
            {
                totalMedicines,
                totalCustomers,
                lowStock,
                totalOrders,      // Số liệu thật từ SQL
                totalRevenue      // Số liệu thật từ SQL
            });
        }
    }
}