using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Batches;
using BatchOptimization.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public BatchController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBatches()
        {
            var bactches = await _context.Batches.ToListAsync();
            return Ok(bactches);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBatch([FromBody] CreateBranchDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var batch = new Models.Batches
            {
                SkuVersionId = dto.SkuVersionId,
                BatchCode = dto.BatchCode,
                BatchSize = dto.BatchSize,
                BatchStatusId = dto.BatchStatusId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
            };
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();
            return Ok(batch);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBatch(int id, [FromBody] UpdateBranchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
                return NotFound();
            batch.BatchCode = dto.BatchCode;
            batch.BatchSize = dto.BatchSize;
            batch.BatchStatusId = dto.BatchStatusId;
            batch.UpdatedAt = DateTime.UtcNow;
            batch.UpdatedBy = userId;
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();
            return Ok(batch);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
                return NotFound();
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            _context.Batches.Remove(batch);
            return Ok();
        }
    }
}
