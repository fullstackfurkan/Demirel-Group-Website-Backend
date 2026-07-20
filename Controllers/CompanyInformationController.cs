using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyInformationController : Controller
    {
        private readonly DemirelGroupDBContext _dbContext;

        public CompanyInformationController(DemirelGroupDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyInformation()
        {
            var companyInformation = await _dbContext.CompanyInformation.FirstOrDefaultAsync();

            if (companyInformation == null)
                return NotFound();

            return Ok(companyInformation);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCompanyInformation([FromBody] CompanyInformation updatedInfo)
        {
            var companyInformation = await _dbContext.CompanyInformation.FirstOrDefaultAsync();

            if (companyInformation == null)
            {
                _dbContext.CompanyInformation.Add(updatedInfo);
            }
            else
            {
                companyInformation.CompanyName = updatedInfo.CompanyName;
                companyInformation.CompanyAdress = updatedInfo.CompanyAdress;
                companyInformation.ContactNumber1 = updatedInfo.ContactNumber1;
                companyInformation.ContactNumber2 = updatedInfo.ContactNumber2;
                companyInformation.CompanyEmail = updatedInfo.CompanyEmail;
                companyInformation.AboutUsText = updatedInfo.AboutUsText;
                companyInformation.FooterText = updatedInfo.FooterText;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Şirket bilgileri başarıyla güncellendi." });
        }
    }
}
