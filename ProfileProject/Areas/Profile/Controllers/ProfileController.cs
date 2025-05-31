using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using ProfileProject.Data;
using ProfileProject.Data.Attributes;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProfileProject.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("profile/")]
    public class ProfileController : Controller
    {
        private IAuthControl _auth;
        public ProfileController(IAuthControl auth)
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
                if (model == null)
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
                if (result) { return RedirectToAction($"info", "profile"); }
                else
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.RepeatPassword),
                        "Something went wrong during account creation");
                    return View(input);
                }
            }
            return View();
        }

        [HttpGet]
        [Route("info/")]
        [Authorize]
        public IActionResult RegisterAddInfo()
        {
            return View();
        }
        [HttpPost]
        [Route("info/")]
        [Authorize]
        public async Task<IActionResult> RegisterAddInfo([FromForm] RegisterAddInfoViewModel input)
        {
            if (ModelState.IsValid)
            {
                await _auth.AddAdditionalInfo(input.Bio, input.Age, input.PFPath);
                return RedirectToAction($"{User.Identity.Name}", "profile");
            }
            return View();
        }

        [HttpGet]
        [Route("edit/{username:required}")]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] string username, 
            [FromServices] IProfileService profileService)  
        {
            AuthorizationResult? result = await _auth.ProveUserOwnership(User, username);
            if(result == null || !result.Succeeded) { return Forbid(); }
            
            ProfileViewModel? profileVM = await profileService.GetByUsername(username);
            EditViewModel editVM = profileService.EditViewModelFromProfile(profileVM);
            editVM.IsAllowedToEdit = true;
            return View(editVM);
        }
        [HttpPost]
        [Route("edit/{username:required}")]
        [Authorize]
        public async Task<IActionResult> Edit(string username, [FromForm] EditViewModel input)
        {
            if(!ModelState.IsValid) { return View(); }

            AuthorizationResult result = await _auth.ProveUserOwnership(User, username);
            if (result == null || !result.Succeeded) { return Forbid(); }

            await _auth.AddAdditionalInfo(input.Bio, input.Age, input.NewPFP, input.ShowAge, input.ShowInSearch);

            return RedirectToAction($"{User.Identity.Name}", "profile");
        }

        [HttpGet]
        [Route("search/")]
        public IActionResult Search()
        {
            return View();
        }

        //Not sure if this method is even used, make intergration test and delete if not needed
        //[HttpGet]
        //[Route("search/{username:required}")]
        //public IActionResult Search(string username)
        //{
        //    return View();
        //}

        [Route("{username:required}")]
        public async Task<IActionResult> GetByUsername(string username, [FromServices] IProfileService profileService)
        {
            ProfileViewModel? profile = await profileService.GetByUsername(username);
            if(profile != null) 
            {
                AuthorizationResult? result = await _auth.ProveUserOwnership(User, username);
                if(result != null && result.Succeeded) 
                { 
                    profile.IsAllowedToEdit = true;
                    ViewData["isOwner"] = true;
                }
                return View(profile); 
            }
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
