using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.SkuVersionMeasurements;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Operator")]
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
        [Authorize(Roles = "Admin,Operator")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSkuMeasurement(int id)
        {
            // Find the record by primary key
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var skuMeasurement = await _context.SkuVersionMeasurements.FindAsync(id);

            // If not found, return 404
            if (skuMeasurement == null)
                return NotFound($"SkuVersionMeasurement with ID {id} not found.");

            // Remove the record from the DbContext
            _context.SkuVersionMeasurements.Remove(skuMeasurement);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 204 No Content (standard for DELETE success)
            return NoContent();
        }
        [Authorize(Roles = "Admin,Operator")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSkuMeasurement(int id, [FromBody] UpdateSkuMeasurementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var skuMeasurement = await _context.SkuVersionMeasurements.FindAsync(id);
            if (skuMeasurement == null)
                return NotFound($"SkuVersionMeasurement with ID {id} not found.");

            skuMeasurement.MeasurementType = dto.MeasurementType;
            skuMeasurement.MeasurementValue = dto.MeasurementValue;

            _context.SkuVersionMeasurements.Update(skuMeasurement);
            await _context.SaveChangesAsync();

            return Ok(skuMeasurement);
        }
    }
}
