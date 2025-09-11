using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Tinters;
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
            var tinters = await _context.Tinters
                .OrderByDescending(t=>t.UpdatedAt)
                .ToListAsync();
            return Ok(tinters);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTinter([FromBody] CreateTinterDto dto)
        {
            if (!ModelState.IsValid)
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
                CreatedAt = DateTime.UtcNow,
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
            var tinter = await _context.Tinters
                .Include(t => t.TinterBatches)
                    .ThenInclude(tb => tb.TinterBatchMeasurements)
                .FirstOrDefaultAsync(t => t.TinterId == id);

            if (tinter == null)
                return NotFound();

            if (tinter.TinterBatches != null && tinter.TinterBatches.Any())
            {
                foreach (var batch in tinter.TinterBatches)
                {
                    if (batch.TinterBatchMeasurements != null && batch.TinterBatchMeasurements.Any())
                    {
                        _context.TinterBatchMeasurements.RemoveRange(batch.TinterBatchMeasurements);
                    }
                }

                // ✅ remove batches themselves after their measurements are removed
                _context.TinterBatches.RemoveRange(tinter.TinterBatches);
            }

            // ✅ finally remove tinter
            _context.Tinters.Remove(tinter);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Tinter and related batches deleted successfully." });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllInfoForTinter(int id)
        {
            var tinter = await _context.Tinters
                .Where(t => t.TinterId == id)
                .Select(t => new
                {
                    t.TinterId,
                    t.PlantId,
                    t.TinterCode,
                    t.IsActive,
                    TinterBatches = t.TinterBatches
                    .Select(tb => new
                    {
                        tb.TinterBatchId,
                        tb.BatchTinterName,
                        tb.TinterBatchCode,
                        tb.IsActive,
                        Measurements = tb.TinterBatchMeasurements
                        .Select(m => new
                        {
                            m.TinterBatchMeasurementId,
                            m.MeasurementType,
                            m.MeasurementValue
                        })
                    })
                })
                .FirstOrDefaultAsync();

            if (tinter == null) return NotFound();
            return Ok(tinter);
        }
        [HttpGet("plant/{plantId:int}")]
        public async Task<IActionResult> GetTintersByPlant(int plantId)
        {
            var tinters = await _context.Tinters
                .Where(t => t.PlantId == plantId)
                .Select(t => new
                {
                    t.TinterId,
                    t.PlantId,
                    t.TinterCode,
                    t.IsActive,
                })
                .ToListAsync();
            return Ok(tinters);
        }
        [Authorize]
        [HttpPost("full/")]
        public async Task<IActionResult> CreateTinterWithBatchesAndMeasurements([FromBody] CreateTinterWithBatchesDto dto)
        {
            if (dto == null || dto.Tinter == null)
                return BadRequest("Tinter details are required.");

            var userIdClaim = User.FindFirst("user_id")?.Value
                 ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ Create Tinter
                var tinter = new Tinters
                {
                    PlantId = dto.Tinter.PlantId,
                    TinterCode = dto.Tinter.TinterCode,
                    IsActive = dto.Tinter.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                };
                _context.Tinters.Add(tinter);
                await _context.SaveChangesAsync();

                // 2️⃣ Create Batches
                foreach (var batchWithMeasurements in dto.Batches)
                {
                    var batchDto = batchWithMeasurements.Batch;
                    var batch = new TinterBatches
                    {
                        TinterId = tinter.TinterId,
                        TinterBatchCode = batchDto.TinterBatchCode,
                        BatchTinterName = batchDto.BatchTinterName,
                        Strength = batchDto.Strength,
                        Comments = batchDto.Comments,
                        IsActive = batchDto.IsActive,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.TinterBatches.Add(batch);
                    await _context.SaveChangesAsync();

                    // 3️⃣ Create Measurements
                    if (batchWithMeasurements.Measurements != null)
                    {
                        foreach (var measurementDto in batchWithMeasurements.Measurements)
                        {
                            var measurement = new TinterBatchMeasurements
                            {
                                TinterBatchId = batch.TinterBatchId,
                                MeasurementType = measurementDto.MeasurementType,
                                MeasurementValue = measurementDto.MeasurementValue
                            };
                            _context.TinterBatchMeasurements.Add(measurement);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Tinter with batches and measurements created successfully", tinterId = tinter.TinterId });

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        } }
}