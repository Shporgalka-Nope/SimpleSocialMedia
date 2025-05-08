using Microsoft.AspNetCore.Mvc;

namespace ProfileProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateNewUser()
        {
            return View();
        }

        
    }
}
