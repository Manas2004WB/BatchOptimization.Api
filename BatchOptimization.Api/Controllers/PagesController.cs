using BatchOptimization.Api.Data;
using BatchOptimization.Api.DTOs.BatchStatus;
using BatchOptimization.Api.DTOs.Pages;
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
    public class PagesController : ControllerBase
    {
        private readonly BatchDbContext _context;
        public PagesController(BatchDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllPages")]
        public async Task<IActionResult> GetAllPages()
        {
            var pages = await _context.Pages.ToListAsync();
            return Ok(pages);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePage([FromBody] CreatePageDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = User.FindFirst("user_id")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID format.");
            var page = new Models.Pages
            {
                PageName = dto.PageName,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
            };
            if (page != null)
            {
                _context.Pages.Add(page);
            }
            await _context.SaveChangesAsync();
            return Ok(page);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            ;
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return Ok(page);
        }
    }
}
