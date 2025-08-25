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
        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> SkuWithVersionMeasurement(int id)
        //{
        //    var sku = await _context.Skus
        //        .Where(s => s.SkuId == id)
        //        .Select(s => new
        //        {
        //            s.SkuId,
        //            s.SkuName,
        //            SkuVersions = s.SkuVersions
        //            .Select(v => new
        //            {
        //                v.SkuVersionId,
        //                v.VersionNumber,
        //                v.VersionName,
        //                Measurements = v.SkuVersionMeasurements
        //                .Select(m => new
        //                {
        //                    m.SkuVersionMeasurementId,
        //                    m.MeasurementType,
        //                    m.MeasurementValue
        //                })
        //            })
        //        })
        //        .FirstOrDefaultAsync();

        //    if (sku == null)
        //        return NotFound($"SKU with ID {id} not found.");

        //    return Ok(sku);
        //}
        //[HttpGet("{id:int}/with-version-measurements")]
        //public async Task<IActionResult> SkuWithVersionMeasurement(int id)
        //{
        //    var sku = await _context.Skus
        //        .Where(s => s.SkuId == id)
        //        .Select(s => new
        //        {
        //            s.SkuId,
        //            s.SkuName,
        //            SkuVersions = s.SkuVersions.Select(v => new
        //            {
        //                v.SkuVersionId,
        //                SkuRevision = v.VersionNumber,
        //                SkuCode = s.SkuName, // Or replace with real SKU Code field if you have one
        //                StdLiquid = new
        //                {
        //                    L = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "liquid_l")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    A = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "liquid_a")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    B = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "liquid_b")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault()
        //                },
        //                PanelColor = new
        //                {
        //                    L = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "panel_l")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    A = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "panel_a")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    B = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "panel_b")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault()
        //                },
        //                SpectroColor = new
        //                {
        //                    L = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "colorimeter_l")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    A = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "colorimeter_a")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault(),
        //                    B = v.SkuVersionMeasurements
        //                        .Where(m => m.MeasurementType == "colorimeter_b")
        //                        .Select(m => m.MeasurementValue)
        //                        .FirstOrDefault()
        //                },
        //                TargetDeltaE= v.SkuVersionMeasurements
        //                    .Where(m => m.MeasurementType == "target_delta_e")
        //                    .Select(m => m.MeasurementValue)
        //                    .FirstOrDefault(),
        //                StdTinters = v.StandardRecipes.Select(r => new
        //                {
        //                    r.TinterId,
        //                    r.Tinter.TinterCode,

        //                }),
        //                v.UpdatedAt,
        //                v.Comments
        //            })
        //        })
        //        .FirstOrDefaultAsync();

        //    if (sku == null)
        //        return NotFound($"SKU with ID {id} not found.");

        //    return Ok(sku);
        //}
        [HttpGet("{plantId}/with-version-measurements")]
        public async Task<IActionResult> SkusWithVersionMeasurements(int plantId)
        {
            var skus = await _context.Skus
                .Where(s => s.PlantId == plantId)
                .Select(s => new
                {
                    s.SkuId,
                    s.SkuName,
                    SkuVersions = s.SkuVersions.Select(v => new
                    {
                        v.SkuVersionId,
                        SkuRevision = v.VersionNumber,
                        SkuCode = s.SkuName, // Or use actual SKU Code if you have one
                        StdLiquid = new
                        {
                            L = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "liquid_l")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            A = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "liquid_a")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            B = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "liquid_b")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault()
                        },
                        PanelColor = new
                        {
                            L = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "panel_l")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            A = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "panel_a")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            B = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "panel_b")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault()
                        },
                        SpectroColor = new
                        {
                            L = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "colorimeter_l")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            A = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "colorimeter_a")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault(),
                            B = v.SkuVersionMeasurements
                                .Where(m => m.MeasurementType == "colorimeter_b")
                                .Select(m => m.MeasurementValue)
                                .FirstOrDefault()
                        },
                        TargetDeltaE = v.SkuVersionMeasurements
                            .Where(m => m.MeasurementType == "target_delta_e")
                            .Select(m => m.MeasurementValue)
                            .FirstOrDefault(),
                        StdTinters = v.StandardRecipes.Select(r => new
                        {
                            r.TinterId,
                            r.Tinter.TinterCode
                        }),
                        v.UpdatedAt,
                        v.Comments
                    })
                })
                .ToListAsync();

            if (!skus.Any())
                return NotFound($"No SKUs found for PlantId {plantId}");

            return Ok(skus);
        }

    }
}
