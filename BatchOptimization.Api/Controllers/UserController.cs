using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.User;
using BatchOptimization.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BatchOptimization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly BatchDbContext _context;

        public UserController(BatchDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .Select(u => new UserReadDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    UserRoleId = u.UserRoleId,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }

        // ✅ POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Username already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new Users
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Email = dto.Email,
                UserRoleId = dto.UserRoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = dto.CreatedBy
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new UserReadDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                UserRoleId = user.UserRoleId,
                IsActive = user.IsActive
            };

            return CreatedAtAction(nameof(GetUsers), new { id = user.UserId }, response);
        }
    }

}
