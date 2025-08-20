using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Shots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShotsController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public ShotsController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBatches()
        {
            var shots = await _context.Shots.ToListAsync();
            return Ok(shots);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateShot([FromBody] CreateShotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var shot = new Models.Shots
            {
                BatchId = dto.BatchId,
                ShotNumber = dto.ShotNumber,
                Comments = dto.Comments,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId
            };
            _context.Shots.Add(shot);
            await _context.SaveChangesAsync();
            return Ok(shot);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShot(int id, [FromBody] UpdateShotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var shot = await _context.Shots.FindAsync(id);
            if (shot == null)
                return NotFound($"Shot with ID {id} not found.");
            shot.Comments = dto.Comments;
            shot.UpdatedAt = DateTime.UtcNow;
                shot.UpdatedBy = userId;
                _context.Shots.Update(shot);
            await _context.SaveChangesAsync();
            return Ok(shot);
        }
    }
}
