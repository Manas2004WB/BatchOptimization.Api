using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Auth;
using BatchOptimization.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BatchOptimization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BatchDbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(BatchDbContext context, JwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.UserRole.RoleName);

            return Ok(new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Username = user.Username,
                Role = user.UserRole.RoleName,
                Email = user.Email,
            });
        }
    }

}
