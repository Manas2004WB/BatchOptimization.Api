using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.StandardRecipes;
using BatchOptimization.Api.DTOs.Tinters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BatchOptimization.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardRecipesController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public StandardRecipesController(BatchDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStandardRecipes()
        {
            var standardRecipes = await _context.StandardRecipes.ToListAsync();
            return Ok(standardRecipes);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStandardRecipeById(int id)
        {
            var recipe = await _context.StandardRecipes.FindAsync(id);
            if (recipe == null)
                return NotFound("Standard Recipe not found.");
            return Ok(recipe);
        }
        [HttpGet("BySku/{skuVersionId}")]
        public async Task<IActionResult> GetBySku(int skuVersionId)
        {
            var recipes = await _context.StandardRecipes
                .Where(r => r.SkuVersionId == skuVersionId)
                .ToListAsync();
            return Ok(recipes);
        }

        [HttpGet("ByTinter/{tinterId}")]
        public async Task<IActionResult> GetByTinter(int tinterId)
        {
            var recipes = await _context.StandardRecipes
                .Where(r => r.TinterId == tinterId)
                .ToListAsync();
            return Ok(recipes);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStandardRecipe([FromBody] CreateStandardRecipesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");

            var standardRecipe = new Models.StandardRecipes
            {
                SkuVersionId = dto.SkuVersionId,
                TinterId = dto.TinterId,
                UpdateNumber = dto.UpdateNumber,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId,
            };

            _context.StandardRecipes.Add(standardRecipe);
            await _context.SaveChangesAsync();
            return Ok(standardRecipe);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStandardRecipe(int id, [FromBody] UpdateStandardRecipesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recipe = await _context.StandardRecipes.FindAsync(id);
            if (recipe == null)
                return NotFound("Standard Recipe not found.");

            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user ID.");

            recipe.SkuVersionId = dto.SkuVersionId;
            recipe.TinterId = dto.TinterId;
            recipe.UpdateNumber = dto.UpdateNumber;
            recipe.UpdatedAt = DateTime.UtcNow;
            recipe.UpdatedBy = userId;

            _context.StandardRecipes.Update(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Standard Recipe updated successfully.", recipe });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStandardRecipe(int id)
        {
            var recipe = await _context.StandardRecipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            _context.StandardRecipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Standard Recipe deleted successfully." });
        }
    }
}