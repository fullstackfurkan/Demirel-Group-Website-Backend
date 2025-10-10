using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class DemirelGroupDBContext : DbContext
    {
        public DemirelGroupDBContext(DbContextOptions<DemirelGroupDBContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
    }
}
