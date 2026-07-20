using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class DemirelGroupDBContext : DbContext
    {
        public DemirelGroupDBContext(DbContextOptions<DemirelGroupDBContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
        public DbSet<ProjectPhotos> ProjectPhotos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
