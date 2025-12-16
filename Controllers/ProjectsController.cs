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
            try
            {
                string baseUrl = "https://api.demirellergroup.com.tr";

                var projects = await _dbContext.Projects
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

                        Photos = p.Photos.Select(ph => new
                        {
                            ph.Id,
                            PhotoUrl = ph.PhotoUrl.StartsWith("http")
                                ? ph.PhotoUrl
                                : baseUrl + ph.PhotoUrl,
                            ph.IsPrimary
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sunucu Hatası", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string baseUrl = "https://api.demirellergroup.com.tr";

            var project = await _dbContext.Projects
                .Where(p => p.Id == id)
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

                    Photos = p.Photos.Select(ph => new
                    {
                        ph.Id,
                        PhotoUrl = ph.PhotoUrl.StartsWith("http")
                            ? ph.PhotoUrl
                            : baseUrl + ph.PhotoUrl,
                        ph.IsPrimary
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}