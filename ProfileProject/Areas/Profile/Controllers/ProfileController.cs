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
        private BasicAuthControl _auth;
        public ProfileController(ApplicationDbContext context, BasicAuthControl auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpGet]
        [Route("signin/")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("signin/")]
        public async Task<IActionResult> SignIn([FromForm] SignInViewModel input)
        {
            if (ModelState.IsValid)
            {
                var model = await _auth.SignInUser(input.Email, input.Password, input.RememberMe);
                if(model == null)
                {
                    ModelState.AddModelError(nameof(input.Password), "Wrong email or password");
                    return View(input);
                }
                return RedirectToAction($"{model.Username}", "profile", new { area = "" });
            }
            return View();
        }

        [HttpGet]
        [Route("register/")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register/")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel input)
        {
            if(ModelState.IsValid)
            {
                bool result = await _auth.AddNewUserWithCookies(input.Username, input.Email, input.Password);
                if (result) { return RedirectToAction($"{input.Username}", "profile", new { area = "" }); }
                else
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.RepeatPassword), 
                        "Something went wrong during account creation");
                    return View(input);
                }
            }
            return View();
        }

        [Route("{username:required}")]
        public async Task<IActionResult> GetByUsername(string username, [FromServices] ProfileService profileService)
        {
            ProfileViewModel? profile = await profileService.GetByUsername(username);
            if(profile != null) { return View(profile); }
            return NotFound();
        }
    }
}
