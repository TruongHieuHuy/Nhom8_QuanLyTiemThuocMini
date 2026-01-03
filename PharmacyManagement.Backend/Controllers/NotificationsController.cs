using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public NotificationsController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] bool unreadOnly = false)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                var query = _context.Notifications
                    .Where(n => n.RecipientType == "All" || 
                               (n.RecipientType == "Employee" && n.EmployeeId.ToString() == userId))
                    .OrderByDescending(n => n.CreatedDate)
                    .AsQueryable();

                if (unreadOnly)
                {
                    query = query.Where(n => !n.IsRead);
                }

                var notifications = await query.Take(50).ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách thông báo", error = ex.Message });
            }
        }

        // GET: api/Notifications/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                var count = await _context.Notifications
                    .Where(n => !n.IsRead && 
                               (n.RecipientType == "All" || 
                               (n.RecipientType == "Employee" && n.EmployeeId.ToString() == userId)))
                    .CountAsync();

                return Ok(new { count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi đếm thông báo chưa đọc", error = ex.Message });
            }
        }

        // PUT: api/Notifications/{id}/mark-read
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { message = "Không tìm thấy thông báo" });

                notification.IsRead = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã đánh dấu đã đọc" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật thông báo", error = ex.Message });
            }
        }

        // PUT: api/Notifications/mark-all-read
        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                var notifications = await _context.Notifications
                    .Where(n => !n.IsRead && 
                               (n.RecipientType == "All" || 
                               (n.RecipientType == "Employee" && n.EmployeeId.ToString() == userId)))
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã đánh dấu tất cả đã đọc" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật thông báo", error = ex.Message });
            }
        }

        // PUT: api/Notifications/reset-all-unread (Temporary for demo/video)
        [HttpPut("reset-all-unread")]
        public async Task<IActionResult> ResetAllUnread()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.IsRead)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = false;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã reset tất cả thông báo về chưa đọc", count = notifications.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi reset thông báo", error = ex.Message });
            }
        }

        // POST: api/Notifications/check-low-stock
        [HttpPost("check-low-stock")]
        public async Task<IActionResult> CheckLowStock()
        {
            try
            {
                var lowStockMedicines = await _context.Medicines
                    .Where(m => m.CurrentStock <= m.MinStockLevel)
                    .ToListAsync();

                var createdCount = 0;
                var deletedCount = 0;

                // 1. Tạo hoặc cập nhật thông báo cho thuốc sắp hết/đã hết
                foreach (var medicine in lowStockMedicines)
                {
                    // Kiểm tra xem đã có thông báo chưa
                    var existingNotification = await _context.Notifications
                        .Where(n => n.NotificationType == "Stock" && 
                                   n.Message.Contains(medicine.Name) &&
                                   !n.IsRead)
                        .FirstOrDefaultAsync();

                    if (existingNotification == null)
                    {
                        var notification = new Notification
                        {
                            Title = medicine.CurrentStock == 0 ? "Cảnh báo hết hàng" : "Cảnh báo tồn kho",
                            Message = medicine.CurrentStock == 0 
                                ? $"Thuốc '{medicine.Name}' đã hết hàng. Vui lòng nhập thêm!"
                                : $"Thuốc '{medicine.Name}' sắp hết hàng (còn {medicine.CurrentStock} {medicine.Unit})",
                            RecipientType = "All",
                            NotificationType = "Stock",
                            IsRead = false,
                            CreatedDate = DateTime.Now
                        };
                        _context.Notifications.Add(notification);
                        createdCount++;
                    }
                    else
                    {
                        // Cập nhật message nếu tồn kho thay đổi
                        var newMessage = medicine.CurrentStock == 0 
                            ? $"Thuốc '{medicine.Name}' đã hết hàng. Vui lòng nhập thêm!"
                            : $"Thuốc '{medicine.Name}' sắp hết hàng (còn {medicine.CurrentStock} {medicine.Unit})";
                        
                        if (existingNotification.Message != newMessage)
                        {
                            existingNotification.Message = newMessage;
                            existingNotification.Title = medicine.CurrentStock == 0 ? "Cảnh báo hết hàng" : "Cảnh báo tồn kho";
                            existingNotification.CreatedDate = DateTime.Now;
                        }
                    }
                }

                // 2. Xóa thông báo cho thuốc đã được nhập kho đủ HOẶC đã bị xóa
                var allStockNotifications = await _context.Notifications
                    .Where(n => n.NotificationType == "Stock" && !n.IsRead)
                    .ToListAsync();

                foreach (var notification in allStockNotifications)
                {
                    // Tìm tên thuốc trong message
                    var startIndex = notification.Message.IndexOf("'") + 1;
                    var endIndex = notification.Message.IndexOf("'", startIndex);
                    if (startIndex > 0 && endIndex > startIndex)
                    {
                        var medicineName = notification.Message.Substring(startIndex, endIndex - startIndex);
                        var medicine = await _context.Medicines
                            .FirstOrDefaultAsync(m => m.Name == medicineName);

                        // Xóa thông báo nếu:
                        // - Thuốc không còn tồn tại (đã bị xóa)
                        // - Thuốc đã có đủ hàng (CurrentStock > MinStockLevel)
                        if (medicine == null || medicine.CurrentStock > medicine.MinStockLevel)
                        {
                            _context.Notifications.Remove(notification);
                            deletedCount++;
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Đã kiểm tra và cập nhật thông báo tồn kho", 
                    created = createdCount,
                    deleted = deletedCount,
                    lowStockCount = lowStockMedicines.Count 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi kiểm tra tồn kho", error = ex.Message });
            }
        }
    }
}
