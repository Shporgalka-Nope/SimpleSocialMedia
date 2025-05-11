using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using ProfileProject.Data;
using ProfileProject.Data.Attributes;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using System.Threading.Tasks;

namespace ProfileProject.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("profile/")]
    public class ProfileController : Controller
    {
        private BasicAuthControl _auth;
        public ProfileController(BasicAuthControl auth)
        {
            _auth = auth;
        }

        [HttpGet]
        [Route("signin/")]
        [OnlyAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("signin/")]
        [OnlyAnonymous]
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
        [OnlyAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register/")]
        [OnlyAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel input)
        {
            if (ModelState.IsValid)
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

        [Route("logout/")]
        public async Task<IActionResult> LogOut([FromServices] SignInManager<IdentityUser> signInManager)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction($"signin", "profile", new { area = "" });
        }
    }
}
