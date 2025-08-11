using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.SkuVersionMeasurements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkuMeasurementController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public SkuMeasurementController(BatchDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkusMeasurements()
        {
            var skusMeasurements = await _context.SkuVersionMeasurements.ToListAsync();
            return Ok(skusMeasurements);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSkuMeasurement([FromBody] CreateSkuMeasurementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var skuVersion = await _context.SkuVersions.FindAsync(dto.SkuVersionId);
            if (skuVersion == null)
                return NotFound($"SkuVersion with ID {dto.SkuVersionId} not found.");
            var skuMeasurement = new Models.SkuVersionMeasurements
            {
                SkuVersionId = dto.SkuVersionId,
                MeasurementType = dto.MeasurementType,
                MeasurementValue = dto.MeasurementValue
            };
            _context.SkuVersionMeasurements.Add(skuMeasurement);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetSkusMeasurements),
                new { id = skuMeasurement.SkuVersionMeasurementId },
                skuMeasurement
            );

        }
    }
}
