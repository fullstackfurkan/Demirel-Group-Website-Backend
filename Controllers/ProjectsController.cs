using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IWebHostEnvironment _env;

        public ProjectsController(DemirelGroupDBContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

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
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var project = await _dbContext.Projects
                .Where(p => p.Id == id)
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
                .FirstOrDefaultAsync();

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProjectDto dto)
        {
            var project = new Project
            {
                Title = dto.Title,
                Category = dto.Category,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Area = dto.Area,
                ApartmentType = dto.ApartmentType,
                ContactNumber = dto.ContactNumber,
                Details = dto.Details,
                Location = dto.Location,
                MapEmbedUrl = dto.MapEmbedUrl,
                Photos = new List<ProjectPhotos>()
            };

            if (dto.NewPhotos != null && dto.NewPhotos.Count > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "ProjectPhotos");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                foreach (var file in dto.NewPhotos)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    project.Photos.Add(new ProjectPhotos
                    {
                        PhotoUrl = "/uploads/ProjectPhotos/" + uniqueFileName,
                        OriginalFileName = file.FileName,
                        IsPrimary = project.Photos.Count == 0 // İlk fotoğraf primary olsun
                    });
                }
            }

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Proje başarıyla eklendi.", id = project.Id });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProjectDto dto)
        {
            var project = await _dbContext.Projects.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();

            project.Title = dto.Title;
            project.Category = dto.Category;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.Area = dto.Area;
            project.ApartmentType = dto.ApartmentType;
            project.ContactNumber = dto.ContactNumber;
            project.Details = dto.Details;
            project.Location = dto.Location;
            project.MapEmbedUrl = dto.MapEmbedUrl;

            // Silinmesi istenen fotoğraflar
            if (dto.DeletedPhotoIds != null && dto.DeletedPhotoIds.Any())
            {
                var photosToDelete = project.Photos.Where(p => dto.DeletedPhotoIds.Contains(p.Id)).ToList();
                foreach (var photo in photosToDelete)
                {
                    var fullPath = Path.Combine(_env.WebRootPath, photo.PhotoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                    project.Photos.Remove(photo);
                }
            }

            // Yeni fotoğraflar ekle
            if (dto.NewPhotos != null && dto.NewPhotos.Any())
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "ProjectPhotos");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                foreach (var file in dto.NewPhotos)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    project.Photos.Add(new ProjectPhotos
                    {
                        PhotoUrl = "/uploads/ProjectPhotos/" + uniqueFileName,
                        OriginalFileName = file.FileName,
                        IsPrimary = project.Photos.Count == 0
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Proje başarıyla güncellendi." });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _dbContext.Projects.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();

            foreach (var photo in project.Photos)
            {
                var fullPath = Path.Combine(_env.WebRootPath, photo.PhotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Proje başarıyla silindi." });
        }
    }
}