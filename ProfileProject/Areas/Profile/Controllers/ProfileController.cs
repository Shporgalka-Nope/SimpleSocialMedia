using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using ProfileProject.Data;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using System.Threading.Tasks;

namespace ProfileProject.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("profile/")]
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;
        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("{username:required}")]
        public async Task<IActionResult> GetByUsername(string username, [FromServices] ProfileService profileService)
        {
            ProfileViewModel profile = await profileService.GetByUsername(username);
            if(profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }
    }
}
