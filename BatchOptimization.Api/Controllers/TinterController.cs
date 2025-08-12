using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Tinters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinterController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public TinterController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTinters()
        {
            var tinters = await _context.Tinters.ToListAsync();
            return Ok(tinters);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTinter([FromBody] CreateTinterDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var tinter = new Models.Tinters
            {
                PlantId = dto.PlantId,
                TinterCode = dto.TinterCode,
                IsActive = dto.IsActive,
                CreatedAt =  DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId, 
            };
            _context.Tinters.Add(tinter);
            await _context.SaveChangesAsync();
            return Ok(tinter);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTinter(int id, [FromBody] UpdateTinterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            
            var tinter = await _context.Tinters.FindAsync(id);
            if (tinter == null)
                return NotFound();
            
            tinter.TinterCode = dto.TinterCode;
            tinter.IsActive = dto.IsActive;
            tinter.UpdatedBy = userId;
            tinter.UpdatedAt = DateTime.UtcNow;
            _context.Tinters.Update(tinter);
            await _context.SaveChangesAsync();
            return Ok(tinter);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTinter(int id)
        {
            var tinter = await _context.Tinters.FindAsync(id);
            if (tinter == null)
                return NotFound();
            _context.Tinters.Remove(tinter);
            await _context.SaveChangesAsync();
            return (Ok(new { message = "Tinter deleted successfully." }));
        }
    }
}
