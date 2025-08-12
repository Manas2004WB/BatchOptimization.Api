using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Sku;
using BatchOptimization.Api.DTOs.SkuVersions;
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

    }
}
