using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Sku;
using BatchOptimization.Api.DTOs.SkuVersions;
using BatchOptimization.Api.DTOs.SkuWithVersion;
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
    public class skuVersionsController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public skuVersionsController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetSkusVersions()
        {
            var skuVersions = await _context.SkuVersions.ToListAsync();
            return Ok(skuVersions);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSkuVersion([FromBody] CreateSkuVersionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var skuVersion = new Models.SkuVersions
            {
                SkuId = dto.SkuId,
                VersionNumber = dto.VersionNumber,
                VersionName = dto.VersionName,
                ProductTypeId = dto.ProductTypeId,
                ColorimeterInstrumentId = dto.ColorimeterInstrumentId,
                IsDefault = dto.IsDefault,
                Comments = dto.Comments,
                IsActive = dto.IsActive,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedAt = DateTime.UtcNow
            };
            _context.SkuVersions.Add(skuVersion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSkusVersions), new { id = skuVersion.SkuVersionId }, skuVersion);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSkuVersion(int id, [FromBody] UpdateSkuVersionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            var skuVersion = await _context.SkuVersions.FindAsync(id);
            if (skuVersion == null)
                return NotFound($"SkuVersion with ID {id} not found.");

            skuVersion.VersionName = dto.VersionName;
            skuVersion.ProductTypeId = dto.ProductTypeId;
            skuVersion.ColorimeterInstrumentId = dto.ColorimeterInstrumentId;
            skuVersion.IsDefault = dto.IsDefault;
            skuVersion.Comments = dto.Comments;
            skuVersion.IsActive = dto.IsActive;

            // Update audit fields
            skuVersion.UpdatedBy = userId;
            skuVersion.UpdatedAt = DateTime.UtcNow;

            _context.SkuVersions.Update(skuVersion);
            await _context.SaveChangesAsync();

            return Ok(skuVersion);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSkuVersion(int id)
        {
            var skuVersion = await _context.SkuVersions.FindAsync(id);
            if (skuVersion == null)
                return NotFound($"SkuVersion with ID {id} not found.");
            _context.SkuVersions.Remove(skuVersion);
            await _context.SaveChangesAsync();
            return NoContent();

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSkuVersionWithMeasurements(int id)
        {
            var skuVersion = await _context.SkuVersions
                .Include(sv => sv.SkuVersionMeasurements) // Load related measurements
                .FirstOrDefaultAsync(sv => sv.SkuVersionId == id);

            if (skuVersion == null)
                return NotFound($"SkuVersion with ID {id} not found.");

            return Ok(skuVersion);
        }
        [HttpGet("{plantId}/with-latest-version")]
        public async Task<ActionResult<IEnumerable<SkuWithVersionsDto>>> GetSkusWithLatestVersion(int plantId)
        {
            var skus = await _context.Skus
                .Where(s => s.PlantId == plantId)
                .Include(s => s.SkuVersions)
                    .ThenInclude(v => v.SkuVersionMeasurements)
                .Include(s => s.SkuVersions)
                    .ThenInclude(v => v.StandardRecipes)
                        .ThenInclude(r => r.Tinter)
                .Include(s => s.SkuVersions) // include batches too
                    .ThenInclude(v => v.Batches)
                .OrderByDescending(s => s.UpdatedAt)
                .ToListAsync();

            var dto = skus.Select(sku =>
            {
                // 👇 pick the latest version (highest revision)
                var latestVersion = sku.SkuVersions
                    .OrderByDescending(v => v.VersionNumber)
                    .FirstOrDefault();

                if (latestVersion == null) return null;

                return new SkuWithVersionsDto
                {
                    SkuId = sku.SkuId,
                    SkuName = sku.SkuName,
                    // return only the latest version as a single-item list
                    SkuVersions = new List<SkuVersionDto>
                    {
                            new SkuVersionDto
                            {
                                SkuVersionId = latestVersion.SkuVersionId,
                                SkuRevision = latestVersion.VersionNumber,
                                SkuCode = sku.SkuName, // or latestVersion.SkuCode if different
                                StdLiquid = MapMeasurement(latestVersion.SkuVersionMeasurements, "liquid"),
                                PanelColor = MapMeasurement(latestVersion.SkuVersionMeasurements, "panel"),
                                SpectroColor = MapMeasurement(latestVersion.SkuVersionMeasurements, "colorimeter"),
                                TargetDeltaE = latestVersion.SkuVersionMeasurements
                                    .FirstOrDefault(m => m.MeasurementType == "target_delta_e")?.MeasurementValue,
                                StdTinters = latestVersion.StandardRecipes.Select(r => new TinterDto
                                {
                                    TinterId = r.TinterId,
                                    TinterCode = r.Tinter.TinterCode
                                }).ToList(),
                                Batches = latestVersion.Batches.Select(b => new SkuBatchDto
                                {
                                    BatchId = b.BatchId,
                                    BatchCode = b.BatchCode,
                                    BatchSize = b.BatchSize,
                                    BatchStatusId = b.BatchStatusId,
                                    CreatedAt = b.CreatedAt,
                                    UpdatedAt = b.UpdatedAt
                                }).ToList(),
                                UpdatedAt = latestVersion.UpdatedAt,
                                Comments = latestVersion.Comments
                            }
                    }
                };
            })
            .Where(dto => dto != null) // filter nulls if any SKU has no versions
            .ToList();

            return Ok(dto);
        }


        [HttpGet("{skuId}/older-versions")]
        public async Task<ActionResult<IEnumerable<SkuVersionDto>>> GetOlderVersions(int skuId)
        {
            var sku = await _context.Skus
                .Where(s => s.SkuId == skuId)
                .Include(s => s.SkuVersions)
                    .ThenInclude(v => v.SkuVersionMeasurements)
                .Include(s => s.SkuVersions)
                    .ThenInclude(v => v.StandardRecipes)
                        .ThenInclude(r => r.Tinter)
                .Include(s => s.SkuVersions)
                    .ThenInclude(v => v.Batches)
                .FirstOrDefaultAsync();

            if (sku == null)
                return NotFound();

            // find latest revision number
            var latestRevision = sku.SkuVersions
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefault()?.VersionNumber;

            if (latestRevision == null)
                return Ok(new List<SkuVersionDto>());

            // filter only older versions
            var olderVersions = sku.SkuVersions
                .Where(v => v.VersionNumber < latestRevision)
                .OrderByDescending(v => v.VersionNumber) // newest older first
                .Select(v => new SkuVersionDto
                {
                    SkuVersionId = v.SkuVersionId,
                    SkuRevision = v.VersionNumber,
                    SkuCode = sku.SkuName, // or v.SkuCode if different
                    StdLiquid = MapMeasurement(v.SkuVersionMeasurements, "liquid"),
                    PanelColor = MapMeasurement(v.SkuVersionMeasurements, "panel"),
                    SpectroColor = MapMeasurement(v.SkuVersionMeasurements, "colorimeter"),
                    TargetDeltaE = v.SkuVersionMeasurements
                        .FirstOrDefault(m => m.MeasurementType == "target_delta_e")?.MeasurementValue,
                    StdTinters = v.StandardRecipes.Select(r => new TinterDto
                    {
                        TinterId = r.TinterId,
                        TinterCode = r.Tinter.TinterCode
                    }).ToList(),
                    Batches = v.Batches.Select(b => new SkuBatchDto
                    {
                        BatchId = b.BatchId,
                        BatchCode = b.BatchCode,
                        BatchSize = b.BatchSize,
                        BatchStatusId = b.BatchStatusId,
                        CreatedAt = b.CreatedAt,
                        UpdatedAt = b.UpdatedAt
                    }).ToList(),
                    UpdatedAt = v.UpdatedAt,
                    Comments = v.Comments
                })
                .ToList();

            return Ok(olderVersions);
        }

        private MeasurementDto MapMeasurement(IEnumerable<SkuVersionMeasurements> measurements, string typePrefix)
        {
            return new MeasurementDto
            {
                L = measurements.FirstOrDefault(m => m.MeasurementType == $"{typePrefix}_l")?.MeasurementValue,
                A = measurements.FirstOrDefault(m => m.MeasurementType == $"{typePrefix}_a")?.MeasurementValue,
                B = measurements.FirstOrDefault(m => m.MeasurementType == $"{typePrefix}_b")?.MeasurementValue
            };
        }
    }
}
