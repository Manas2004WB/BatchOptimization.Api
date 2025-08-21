using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.ShotTinters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShotTintersController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public ShotTintersController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetShotTinters()
        {
            var shotTinters = _context.ShotTinters.ToListAsync();
            return Ok(shotTinters);
        }
        [HttpGet("ByShot/{shotId:int}")]
        public async Task<IActionResult> GetTintersByShot(int shotId)
        {
            var tinters = await _context.ShotTinters
                .Where(t => t.ShotId == shotId)
                .ToListAsync();

            if (!tinters.Any())
                return NotFound("No tinters found for this shot.");

            return Ok(tinters);
        }
        [HttpGet("ByBatch/{batchId:int}")]
        public async Task<IActionResult> GetTintersByBatch(int batchId)
        {
            var tinters = await _context.ShotTinters
                .Where(t => t.TinterBatchId == batchId)
                .ToListAsync();

            if (!tinters.Any())
                return NotFound("No tinters found for this batch.");

            return Ok(tinters);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateShotTinter([FromBody] CreateShotTinterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var shotTinter = new Models.ShotTinters
            {
                ShotId = dto.ShotId,
                TinterBatchId = dto.TinterBatchId,
                TinterWeight = dto.TinterWeight,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
            };
            _context.ShotTinters.Add(shotTinter);

            await _context.SaveChangesAsync();
            return Ok(shotTinter);
        }
        [Authorize]
        [HttpPost("Bulk")]
        public async Task<IActionResult> CreateShotTinters([FromBody] List<CreateShotTinterDto> dtos)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID.");

            var shotTinters = dtos.Select(dto => new Models.ShotTinters
            {
                ShotId = dto.ShotId,
                TinterBatchId = dto.TinterBatchId,
                TinterWeight = dto.TinterWeight,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
            }).ToList();

            await _context.ShotTinters.AddRangeAsync(shotTinters);
            await _context.SaveChangesAsync();

            return Ok(shotTinters);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShotTinter(int id, [FromBody] UpdateShotTinterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");


            var shotTinter = await _context.ShotTinters.FindAsync(id);
            if (shotTinter == null)
                return NotFound();

            shotTinter.TinterBatchId = dto.TinterBatchId;
            shotTinter.TinterWeight = dto.TinterWeight;
            _context.ShotTinters.Update(shotTinter);
            await _context.SaveChangesAsync();
            return Ok(shotTinter);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShotTinter(int id)
        {
            var shotTinter = await _context.ShotTinters.FindAsync(id);
            if (shotTinter == null)
                return NotFound();
            _context.ShotTinters.Remove(shotTinter);
            await _context.SaveChangesAsync();
            return Ok(shotTinter);
        }
    }
}

