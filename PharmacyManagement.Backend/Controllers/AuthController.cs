using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace PharmacyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public AuthController(IConfiguration configuration, PharmacyManagement.Data.PharmacyContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate user credentials (mock implementation)
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return Unauthorized();

            // In real application, verify credentials against database
            var user = new
            {
                id = 1,
                username = request.Username,
                email = request.Username + "@pharmacy.com",
                role = "Admin"
            };

            var token = GenerateJwtToken(user);
            return Ok(new { token, user });
        }

        private string GenerateJwtToken(dynamic user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
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
