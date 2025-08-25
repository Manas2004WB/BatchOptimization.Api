using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.TinterBatches;
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
    public class TinterBatchesController : ControllerBase
    {
        private readonly BatchDbContext _context;

        public TinterBatchesController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTinterBatches()
        {
            var tinterBatches = await _context.TinterBatches.ToListAsync();
            return Ok(tinterBatches);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTinterBatchesForTinterId(int id)
        {
            // fetch tinter batches by TinterId
            var tinterBatches = await _context.TinterBatches
                .Where(tb => tb.TinterId == id)
                .OrderByDescending(tb => tb.CreatedAt) // optional: newest first
                .ToListAsync();

            if (tinterBatches == null || !tinterBatches.Any())
                return NotFound($"No batches found for TinterId {id}");

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


        // ✅ POST: api/TinterBatches/with-measurements
        [HttpPost("with-measurements")]
        [Authorize]
        public async Task<IActionResult> CreateBatchWithMeasurements(
            [FromBody] CreateTinterBatchWithMeasurementsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔹 Get user ID from token
            var userIdClaim = User.FindFirst("user_id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user_id claim.");

            // 1️⃣ Create the Batch (entity should be singular: TinterBatch)
            var batch = new Models.TinterBatches
            {
                TinterId = dto.TinterId,
                TinterBatchCode = dto.TinterBatchCode,
                BatchTinterName = dto.BatchTinterName,
                Strength = dto.Strength,
                Comments = dto.Comments,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            _context.TinterBatches.Add(batch);
            await _context.SaveChangesAsync(); // Save first to generate BatchId

            // 2️⃣ Create Measurements
            foreach (var m in dto.Measurements)
            {
                var measurement = new Models.TinterBatchMeasurements
                {
                    TinterBatchId = batch.TinterBatchId, // ✅ attach generated id
                    MeasurementType = m.MeasurementType,
                    MeasurementValue = m.MeasurementValue
                };

                _context.TinterBatchMeasurements.Add(measurement);
            }

            await _context.SaveChangesAsync();

            // 3️⃣ Return clean response (DTO instead of raw EF entity)
            return Ok(new
            {
                BatchId = batch.TinterBatchId,
                TinterBatchCode = batch.TinterBatchCode,
                batch.TinterId,
                batch.BatchTinterName,
                batch.Strength,
                batch.Comments,
                batch.IsActive,
                batch.CreatedAt,
                batch.UpdatedAt,
                Measurements = dto.Measurements
            });
        }
        [HttpGet("{tinterId:int}/with-measurements")]
        public async Task<IActionResult> GetTinterBatchesWithMeasurements(int tinterId)
        {
            // 🔹 Fetch batches and include measurements
            var batches = await _context.TinterBatches
                .Where(b => b.TinterId == tinterId)
                .Select(b => new
                {
                    b.TinterBatchId,
                    b.TinterBatchCode,
                    b.BatchTinterName,
                    b.Strength,
                    b.Comments,
                    b.IsActive,
                    b.CreatedAt,
                    b.UpdatedAt,
                    Measurements = b.TinterBatchMeasurements
                        .Select(m => new
                        {
                            m.TinterBatchMeasurementId,
                            m.MeasurementType,
                            m.MeasurementValue
                        }).ToList()
                })
                .ToListAsync();

            if (batches == null || !batches.Any())
                return NotFound($"No batches found for TinterId {tinterId}");

            return Ok(batches);
        }


    }
};
