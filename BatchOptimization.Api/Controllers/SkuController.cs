using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Plant;
using BatchOptimization.Api.DTOs.Sku;
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
    public class SkuController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public SkuController(BatchDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkus()
        {
            var skus = await _context.Skus.ToListAsync();
            return Ok(skus);
        }
        [HttpGet("/get1/{id:int}")]
        public async Task<IActionResult> GetSkuById(int id)
        {
            var sku = await _context.Skus
                .Where(s => s.SkuId == id)
                .Select(s => new {
                    s.SkuId,
                    s.SkuName,
                    SkuVersions = s.SkuVersions
                    .Select(v => new {
                        v.SkuVersionId,
                        v.VersionNumber,
                        v.CreatedAt
                    })
                })
                .FirstOrDefaultAsync();

            if (sku == null) return NotFound();
            return Ok(sku);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSku([FromBody] CreateSkuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var sku = new Models.Skus
            {
                PlantId = dto.PlantId,
                SkuName = dto.SkuName,
                IsActive = true,  // default
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                SkuVersions = new List<SkuVersions>()
            };
            _context.Skus.Add(sku);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSkuById), new { id = sku.SkuId }, sku);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSku(int id)
        {
            var sku = await _context.Skus.FindAsync(id);
            if (sku == null)
                return NotFound();
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            sku.UpdatedBy = userId;
            sku.UpdatedAt = DateTime.UtcNow;
            _context.Skus.Remove(sku);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSku(int id, [FromBody] CreateSkuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var sku = await _context.Skus.FindAsync(id);
            if (sku == null)
                return NotFound();
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            sku.PlantId = dto.PlantId;
            sku.SkuName = dto.SkuName;
            sku.UpdatedBy = userId;
            sku.UpdatedAt = DateTime.UtcNow;
            _context.Skus.Update(sku);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPost("with-measurements")]
        public async Task<IActionResult> CreateSkuWithVersion([FromBody] CreateSkuWithVersionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID.");

            // CASE 1: Check if SKU exists
            var existingSku = await _context.Skus
                .Include(s => s.SkuVersions)
                .FirstOrDefaultAsync(s => s.SkuName == dto.SkuCode && s.PlantId == dto.PlantId);

            Skus sku;
            int newVersionNumber;

            if (existingSku == null)
            {
                // Create new SKU
                sku = new Skus
                {
                    PlantId = dto.PlantId,
                    SkuName = dto.SkuCode,
                    IsActive = true,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedBy = userId,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Skus.Add(sku);
                await _context.SaveChangesAsync();

                newVersionNumber = 1;
            }
            else
            {
                sku = existingSku;
                newVersionNumber = sku.SkuVersions.Max(v => v.VersionNumber) + 1;
            }

            // Create SKU Version
            var skuVersion = new SkuVersions
            {
                SkuId = sku.SkuId,
                VersionNumber = newVersionNumber,
                VersionName = $"{dto.SkuCode}_V{newVersionNumber}",
                ProductTypeId = 1, // ⚠️ you might want this from dto
                ColorimeterInstrumentId = 1, // ⚠️ or from dto
                IsDefault = true,
                Comments = dto.Comments,
                IsActive = true,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedAt = DateTime.UtcNow
            };
            _context.SkuVersions.Add(skuVersion);
            await _context.SaveChangesAsync();

            // Add Measurements
            var measurements = new List<SkuVersionMeasurements>
            {
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "liquid_l", MeasurementValue = dto.StdLiquid.L },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "liquid_a", MeasurementValue = dto.StdLiquid.A },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "liquid_b", MeasurementValue = dto.StdLiquid.B },

                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "panel_l", MeasurementValue = dto.PanelColor.L },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "panel_a", MeasurementValue = dto.PanelColor.A },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "panel_b", MeasurementValue = dto.PanelColor.B },

                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "colorimeter_l", MeasurementValue = dto.SpectroColor.L },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "colorimeter_a", MeasurementValue = dto.SpectroColor.A },
                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "colorimeter_b", MeasurementValue = dto.SpectroColor.B },

                new() { SkuVersionId = skuVersion.SkuVersionId, MeasurementType = "target_delta_e", MeasurementValue = dto.TargetDeltaE }
            };

            _context.SkuVersionMeasurements.AddRange(measurements);

            // Add Standard Recipe (Tinters)
            if (dto.StdTinters != null)
            {
                foreach (var tinter in dto.StdTinters)
                {
                    _context.StandardRecipes.Add(new StandardRecipes
                    {
                        SkuVersionId = skuVersion.SkuVersionId,
                        TinterId = tinter.TinterId,   
                        UpdateNumber = 1,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedBy = userId,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }


            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = existingSku == null ? "New SKU created with version 1" : "New version added to existing SKU",
                SkuId = sku.SkuId,
                SkuVersionId = skuVersion.SkuVersionId
            });
        }

    }
}
