using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;

namespace ProfileProject.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class AuthController : Controller
    {
        private IAuthControl _auth;
        public AuthController(IAuthControl auth)
        {
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
        public IActionResult SignIn(ProfileViewModel input)
        {
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
                if(result)
                {
                    return RedirectToAction($"{input.Username}", "profile", new {area = ""});
                }
            }
            return View();
        }
    }
}
