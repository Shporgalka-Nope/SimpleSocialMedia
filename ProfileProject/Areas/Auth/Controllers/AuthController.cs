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
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(ProfileViewModel input)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] ProfileViewModel input)
        {
            if(ModelState.IsValid)
            {
                await _auth.AddNewUserWithCookies(input.Nickname, input.Email, input.Password);
            }
            return View();
        }
    }
}
