using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PharmacyContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(PharmacyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Vui lòng nhập tài khoản và mật khẩu");

            // 1. Tìm user trong database
            // Lưu ý: So sánh PasswordAccount (tên mới) thay vì PasswordHash (tên cũ)
            var user = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordAccount == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu!" });

            if (!user.IsActive)
                return Unauthorized(new { message = "Tài khoản đã bị khóa!" });

            // 2. Cập nhật lần đăng nhập cuối
            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // 3. Tạo Token
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token = token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    role = user.Role,
                    fullName = user.Username 
                }
            });
        }

        private string GenerateJwtToken(UserAccount user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            
            // Key dự phòng nếu chưa cấu hình
            if (string.IsNullOrEmpty(secretKey)) secretKey = "DayLaKhoaBiMatMacDinhChoDuAnNay_KhongNenDeLoRaNgoai";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString()), 
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}