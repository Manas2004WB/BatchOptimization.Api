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
        //[HttpPost]
        //public async Task<IActionResult> CreateBatch([FromBody] CreateBranchDto dto)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var userIdClaim = User.FindFirst("user_id")?.Value
        //          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userIdClaim))
        //        return Unauthorized("User ID not found in token.");

        //    if (!int.TryParse(userIdClaim, out var userId))
        //        return Unauthorized("Invalid user ID format.");
        //    var batch = new Models.Batches
        //    {
        //        SkuVersionId = dto.SkuVersionId,
        //        BatchCode = dto.BatchCode,
        //        BatchSize = dto.BatchSize,
        //        BatchStatusId = dto.BatchStatusId,
        //        CreatedAt = DateTime.UtcNow,
        //        CreatedBy = userId,
        //        UpdatedAt = DateTime.UtcNow,
        //        UpdatedBy = userId,
        //    };
        //    _context.Batches.Add(batch);
        //    await _context.SaveChangesAsync();
        //    return Ok(batch);
        //}
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
            return Ok("Hogaya bhai delete");
        }
        [HttpGet("{id:int}/ByPlantId")]
        public async Task<IActionResult> GetBatchesForPlant(int id)
        {
            var batches = await _context.Batches
                .Where(b => b.SkuVersion.Sku.PlantId == id)
                .Select(b => new
                {
                    b.BatchId,
                    b.BatchCode,
                    b.BatchSize,
                    b.BatchStatusId,
                    b.UpdatedAt,
                    b.UpdatedBy,
                    SkuVersion = new
                    {
                        b.SkuVersion.SkuVersionId,
                        b.SkuVersion.VersionName,
                        Sku = new
                        {
                            b.SkuVersion.Sku.SkuId,
                            b.SkuVersion.Sku.SkuName
                        }
                    }
                })
                .ToListAsync();

            return Ok(batches);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBatchWithVersion([FromBody] CreateBatchFrontendDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            // 1️⃣ Get latest SKU version for this SKU
            var latestSkuVersion = await _context.SkuVersions
                .Where(v => v.SkuId == dto.SkuId)
                .OrderByDescending(v => v.CreatedAt) // assumes CreatedAt or VersionNo field exists
                .FirstOrDefaultAsync();

             var targetDeltaE = await _context.SkuVersionMeasurements
            .Where(m => m.SkuVersionId == latestSkuVersion.SkuVersionId &&
                        m.MeasurementType == "target_delta_e")
            .Select(m => (double?)m.MeasurementValue)
            .FirstOrDefaultAsync();


            if (latestSkuVersion == null)
                return NotFound("No version found for this SKU.");

            // 2️⃣ Create Batch entity
            var newBatch = new Batches
            {
                SkuVersionId = latestSkuVersion.SkuVersionId,
                BatchCode = dto.BatchCode,
                BatchSize = dto.BatchSize,
                BatchStatusId = 1, // Default Active
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId, // TODO: replace with logged-in userId
                UpdatedBy = userId
            };

            _context.Batches.Add(newBatch);
            await _context.SaveChangesAsync();

            // 3️⃣ Return response with auto-filled Target ΔE
            return Ok(new
            {
                newBatch.BatchId,
                newBatch.BatchCode,
                newBatch.BatchSize,
                TargetDeltaE = targetDeltaE,
                SkuVersion = latestSkuVersion.VersionName, // comes from sku_versions table
            });
        }
    }
}
