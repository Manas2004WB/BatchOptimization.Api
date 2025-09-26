using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.BatchStatus;
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
    public class BatchStatusController : ControllerBase
    {

        private readonly BatchDbContext _context;
        public BatchStatusController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetBatchStatus")]
        public async Task<IActionResult> GetBatchStatus()
        {
            var batchStatus = await _context.BatchStatuses.ToListAsync();
            return Ok(batchStatus);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBatchStatus([FromBody] CreateBatchStatusDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var batchStatus = new Models.BatchStatuses
            {
                StatusName = dto.StatusName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
            };
            if(batchStatus != null)
            {
                _context.BatchStatuses.Add(batchStatus);
            } 
            await _context.SaveChangesAsync();
            return Ok(batchStatus);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBatchStatus(int id)
        {
            var batchStatus = await _context.BatchStatuses.FindAsync(id);
            if (batchStatus == null)
            {
                return NotFound();
            }
            ;
            _context.BatchStatuses.Remove(batchStatus);
            await _context.SaveChangesAsync();
            return Ok(batchStatus);
        }
    }
}
