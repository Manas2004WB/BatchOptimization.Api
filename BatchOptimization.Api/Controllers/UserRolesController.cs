using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Tinters;
using BatchOptimization.Api.DTOs.UserRoleDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public UserRolesController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            return Ok(userRoles);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ Get logged-in userId from claims
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            // ✅ Fetch user from DB
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return Unauthorized("User not found.");

            // ✅ Proceed to create role
            var userRole = new Models.UserRoles
            {
                RoleName = dto.RoleName,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return Ok(userRole);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        { var userRole = await _context.UserRoles.FindAsync(id);
            if(userRole == null)
            {
                return NotFound();
            };
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return Ok(userRole);
        }
        [HttpGet("RolenameById/{id:int}")]
        public async Task<IActionResult> GetRoleNameById(int id)
        {
            // Find the role by id
            var userRole = await _context.UserRoles.FindAsync(id);

            // If not found, return 404
            if (userRole == null)
            {
                return NotFound(new { message = $"Role with id {id} not found." });
            }

            // Otherwise, return the role name
            return Ok(userRole.RoleName);
        }

    }
}
