using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
