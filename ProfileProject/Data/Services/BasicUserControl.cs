using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;

namespace ProfileProject.Data.Services
{
    public class BasicAuthControl : IAuthControl
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private ILogger<BasicAuthControl> _logger;
        public BasicAuthControl(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] ILogger<BasicAuthControl> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<bool> AddNewUserWithCookies(string Nickname, string Email, string Password)
        {
            bool hasPassed = true;
            _logger.LogInformation("New register request detected");
            var newUser = new IdentityUser
            {
                UserName = Nickname,
                Email = Email
            };

            var result = await _userManager.CreateAsync(newUser, Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Identity user created");
                var claims = new Claim[]
                {
                    new Claim("Nickname", Nickname),
                    new Claim("CreationDate", $"{DateTime.UtcNow}"),
                    new Claim("ViolatinsCount", "0")
                };
                await _userManager.AddClaimsAsync(newUser, claims);
                await _signInManager.SignInAsync(newUser, false);
            }
            else 
            {
                hasPassed = false;
                _logger.LogWarning("Identity user creation failed"); 
            }
            return hasPassed;
        }
    }
}
