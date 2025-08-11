using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.Plant;
using BatchOptimization.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BatchOptimization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly BatchDbContext _context;

        public PlantController(BatchDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
                return NotFound();

            return Ok(plant);
        }


        [HttpGet]
        public async Task<IActionResult> GetPlants()
        {
            var plants = await _context.Plants.ToListAsync();
            return Ok(plants);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePlant([FromBody] CreatePlantDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.PlantName))
                return BadRequest("Valid plant data is required.");

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            var plant = new Plants
            {
                PlantName = dto.PlantName,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _context.Plants.Add(plant);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UQ__plants__2D642453") == true)
                    return Conflict("A plant with this name already exists.");
                throw;
            }

            return CreatedAtAction(nameof(GetPlant), new { id = plant.PlantId }, plant);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
                return NotFound();
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            plant.UpdatedBy = userId;
            plant.UpdatedAt = DateTime.UtcNow;
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] CreatePlantDto dto)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
                return NotFound();

            var userIdClaim = User.FindFirst("user_id")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            plant.PlantName = dto.PlantName;
            plant.IsActive = dto.IsActive;
            plant.UpdatedBy = userId;
            plant.UpdatedAt = DateTime.UtcNow;

            _context.Plants.Update(plant); // ✅ only update
            await _context.SaveChangesAsync();

            return Ok(plant);
        }

    }
}
