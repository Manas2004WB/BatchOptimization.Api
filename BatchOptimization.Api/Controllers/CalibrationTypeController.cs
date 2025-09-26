using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.BatchStatus;
using BatchOptimization.Api.DTOs.CalibrationTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibrationTypeController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public CalibrationTypeController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetCalibrationTypes")]
        public async Task<IActionResult> GetCalibrationStatus()
        {
            var calibrationStatus = await _context.CalibrationType.ToListAsync();
            return Ok(calibrationStatus);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCalibrationTypes([FromBody] CreateCalibrationTypesDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var calibrationType = new Models.CalibrationType
            {
                CalibrationTypeName = dto.CalibrationTypeName,
            };
            if (calibrationType != null)
            {
                _context.CalibrationType.Add(calibrationType);
            }
            await _context.SaveChangesAsync();
            return Ok(calibrationType);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCalibrationType(int id)
        {
            var calibrationType = await _context.CalibrationType.FindAsync(id);
            if (calibrationType == null)
            {
                return NotFound();
            }
            ;
            _context.CalibrationType.Remove(calibrationType);
            await _context.SaveChangesAsync();
            return Ok(calibrationType);
        }
    }
}
