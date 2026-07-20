using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Utilities
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DemirelGroupDBContext>();

            try
            {
                context.Database.Migrate();

                if (!context.Users.Any(u => u.Username == "admin"))
                {
                    context.Users.Add(new User
                    {
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
                    });
                }

                if (!context.CompanyInformation.Any())
                {
                    context.CompanyInformation.Add(new CompanyInformation
                    {
                        CompanyName = "Demireller Group",
                        CompanyAdress = "İstanbul, Türkiye",
                        ContactNumber1 = "0555 555 55 55",
                        ContactNumber2 = "0555 555 55 56",
                        CompanyEmail = "info@demirellergroup.com.tr",
                        AboutUsText = "Demireller Group olarak yılların tecrübesiyle inşaat ve emlak sektöründe hizmet veriyoruz.",
                        FooterText = "© 2026 Demireller Group. Tüm hakları saklıdır."
                    });
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database seeding failed: {ex.Message}");
            }
        }
    }
}
