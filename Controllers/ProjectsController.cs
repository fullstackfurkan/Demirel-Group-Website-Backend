using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : Controller
    {
        private readonly DemirelGroupDBContext _dbContext;

        public ProjectsController(DemirelGroupDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _dbContext.Projects
                .Include(p => p.Photos)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Category,
                    StartDate = p.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = p.EndDate.ToString("yyyy-MM-dd"),
                    p.Area,
                    p.ApartmentType,
                    p.ContactNumber,
                    p.Details,
                    p.Location,
                    p.MapEmbedUrl,
                    p.Photos
                })
                .ToListAsync();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Photos)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Category,
                    StartDate = p.StartDate.ToString("dd MMM yyyy", new CultureInfo("tr-TR")),
                    EndDate = p.EndDate.ToString("dd MMM yyyy", new CultureInfo("tr-TR")),
                    p.Area,
                    p.ApartmentType,
                    p.ContactNumber,
                    p.Details,
                    p.Location,
                    p.MapEmbedUrl,
                    p.Photos
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}
