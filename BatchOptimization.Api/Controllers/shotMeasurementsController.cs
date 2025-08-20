using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.ShotsMeasurements;
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
    public class shotMeasurementsController : ControllerBase
    {

        private readonly BatchDbContext _context;
        public shotMeasurementsController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBatches()
        {
            var shotMeasurements = await _context.ShotMeasurements.ToListAsync();
            return Ok(shotMeasurements);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateShotMeasurement([FromBody] CreateShotMeasurementDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var measurement = new Models.ShotMeasurements
            {
                ShotId = dto.ShotId,
                MeasurementType = dto.MeasurementType,
                MeasurementValue = dto.MeasurementValue
            };
            _context.ShotMeasurements.Add(measurement);
            await _context.SaveChangesAsync();
            return Ok(measurement);
        }
    }
}
