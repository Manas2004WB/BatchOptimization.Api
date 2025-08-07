using BatchOptimization.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

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

        [HttpGet]
        public async Task<IActionResult> GetPlants()
        {
            var plants = await _context.Plants.ToListAsync();
            return Ok(plants);
        }
    }
}
