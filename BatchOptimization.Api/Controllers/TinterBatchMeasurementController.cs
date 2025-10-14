using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Sku;
using BatchOptimization.Api.DTOs.TinterBatches;
using BatchOptimization.Api.DTOs.TinterBatchMeasurement;
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
    public class TinterBatchMeasurementController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public TinterBatchMeasurementController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTinterBatchMeasurement()
        {
            var tinterBatchMeasurements = await _context.TinterBatchMeasurements.ToListAsync();
            return Ok(tinterBatchMeasurements);
        }
        [Authorize(Roles = "Admin,Operator")]
        [HttpPost]
        public async Task<IActionResult> CreateTinterBatchMeasurement([FromBody] CreateTinterBatchMeasurementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var tinterBatchMeasurement = new Models.TinterBatchMeasurements
            {
                TinterBatchId = dto.TinterBatchId,
                MeasurementType = dto.MeasurementType,
                MeasurementValue = dto.MeasurementValue
            };
            _context.TinterBatchMeasurements.Add(tinterBatchMeasurement);
            await _context.SaveChangesAsync();
            return Ok(tinterBatchMeasurement);
        }
        [Authorize(Roles = "Admin,Operator")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTinterBatchMeasurement(int id, [FromBody] UpdateTinterBatchMeasurementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            
            var tinterBatchMeasurement = await _context.TinterBatchMeasurements.FindAsync(id);
            if (tinterBatchMeasurement == null)
                return NotFound();
            tinterBatchMeasurement.MeasurementType = dto.MeasurementType;
            tinterBatchMeasurement.MeasurementValue = dto.MeasurementValue;
            _context.TinterBatchMeasurements.Update(tinterBatchMeasurement);
            await _context.SaveChangesAsync();
            return Ok(tinterBatchMeasurement);
        }
        [Authorize(Roles = "Admin,Operator")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTinterBatchMeasurement(int id)
        {
            var tinterBatchMeasurement = await _context.TinterBatchMeasurements.FindAsync(id);
            if (tinterBatchMeasurement == null)
                return NotFound();
            _context.TinterBatchMeasurements.Remove(tinterBatchMeasurement);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
