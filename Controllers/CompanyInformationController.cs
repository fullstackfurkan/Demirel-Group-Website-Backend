using backend.Data;
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
            var componyInformation = await _dbContext.CompanyInformation.FirstOrDefaultAsync();

            if (componyInformation == null)
                return NotFound();


            return Ok(componyInformation);
        }
    }
}
