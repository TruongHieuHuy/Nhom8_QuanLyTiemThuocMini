using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly PharmacyContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(PharmacyContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/profile
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst("id");
                if (userId == null)
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                if (!int.TryParse(userId.Value, out int id))
                    return BadRequest(new { message = "ID người dùng không hợp lệ" });

                var user = await _context.UserAccounts.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "Người dùng không tồn tại" });

                var profileDTO = new UserProfileDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    AvatarUrl = user.AvatarUrl,
                    Role = user.Role
                };

                return Ok(new { message = "Lấy thông tin thành công", data = profileDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // PUT: api/profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO profileDTO)
        {
            try
            {
                var userId = User.FindFirst("id");
                if (userId == null)
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                if (!int.TryParse(userId.Value, out int id))
                    return BadRequest(new { message = "ID người dùng không hợp lệ" });

                var user = await _context.UserAccounts.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "Người dùng không tồn tại" });

                // Cập nhật thông tin
                if (!string.IsNullOrEmpty(profileDTO.FullName))
                    user.FullName = profileDTO.FullName;

                if (!string.IsNullOrEmpty(profileDTO.Email))
                    user.Email = profileDTO.Email;

                if (!string.IsNullOrEmpty(profileDTO.PhoneNumber))
                    user.PhoneNumber = profileDTO.PhoneNumber;

                if (!string.IsNullOrEmpty(profileDTO.Address))
                    user.Address = profileDTO.Address;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thông tin thành công", data = new { user.Id, user.FullName, user.Email, user.PhoneNumber, user.Address } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }

        // POST: api/profile/upload-avatar
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            try
            {
                var userId = User.FindFirst("id");
                if (userId == null)
                    return Unauthorized(new { message = "Không thể xác định người dùng" });

                if (!int.TryParse(userId.Value, out int id))
                    return BadRequest(new { message = "ID người dùng không hợp lệ" });

                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Vui lòng chọn file ảnh" });

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (Array.IndexOf(allowedExtensions, fileExtension) == -1)
                    return BadRequest(new { message = "Chỉ hỗ trợ định dạng: jpg, jpeg, png, gif" });

                // Kiểm tra kích thước file (max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                    return BadRequest(new { message = "Kích thước file không được vượt quá 5MB" });

                var user = await _context.UserAccounts.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "Người dùng không tồn tại" });

                // Tạo thư mục uploads nếu chưa tồn tại
                var uploadsDir = Path.Combine(_environment.ContentRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                // Tạo tên file độc nhất
                var fileName = $"{id}_{DateTime.Now.Ticks}{fileExtension}";
                var filePath = Path.Combine(uploadsDir, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật URL avatar
                var avatarUrl = $"/uploads/avatars/{fileName}";
                user.AvatarUrl = avatarUrl;
                await _context.SaveChangesAsync();

                // Trả về URL đầy đủ để frontend có thể display ảnh ngay
                var fullAvatarUrl = $"{Request.Scheme}://{Request.Host}{avatarUrl}";
                return Ok(new { message = "Cập nhật ảnh đại diện thành công", data = new { avatarUrl = fullAvatarUrl } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi: " + ex.Message });
            }
        }
    }
}
