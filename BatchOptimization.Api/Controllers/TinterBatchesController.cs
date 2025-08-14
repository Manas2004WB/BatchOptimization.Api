using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.TinterBatches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinterBatchesController : ControllerBase
    {
        private readonly BatchDbContext _context;

        public TinterBatchesController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTinters()
        {
            var tinterBatches = await _context.TinterBatches.ToListAsync();
            return Ok(tinterBatches);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTinterBatch([FromBody] DTOs.TinterBatches.CreateTinterBatchesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var tinterBatch = new Models.TinterBatches
            {
                TinterId = dto.TinterId,
                TinterBatchCode = dto.TinterBatchCode,
                BatchTinterName = dto.BatchTinterName,
                Strength = dto.Strength,
                Comments = dto.Comments,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedAt = DateTime.UtcNow
            };
            _context.TinterBatches.Add(tinterBatch);
            await _context.SaveChangesAsync();
            return Ok(tinterBatch);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTinterBatch(int id, [FromBody] UpdateTinterBatchDto dto)
        {
            var tinterBatch = await _context.TinterBatches.FindAsync(id);

            if (tinterBatch == null)
                return NotFound($"Tinter batch with ID {id} not found.");

            // Update fields
            tinterBatch.TinterId = dto.TinterId;
            tinterBatch.TinterBatchCode = dto.TinterBatchCode;
            tinterBatch.BatchTinterName = dto.BatchTinterName;
            tinterBatch.Strength = dto.Strength;
            tinterBatch.Comments = dto.Comments;
            tinterBatch.IsActive = dto.IsActive;

            // Audit fields
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user_id claim.");

            tinterBatch.UpdatedBy = userId;
            tinterBatch.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(tinterBatch);
        }
        [Authorize]
        [HttpDelete("hard/{id:int}")]
        public async Task<IActionResult> HardDeleteTinterBatch(int id)
        {
            var tinterBatch = await _context.TinterBatches.FindAsync(id);

            if (tinterBatch == null)
                return NotFound($"Tinter batch with ID {id} not found.");

            _context.TinterBatches.Remove(tinterBatch);
            await _context.SaveChangesAsync();

            return Ok($"Tinter batch with ID {id} deleted permanently.");
        }
    }
};
