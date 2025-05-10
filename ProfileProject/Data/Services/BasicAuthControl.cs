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
        private IWebHostEnvironment _env;
        public BasicAuthControl(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] ILogger<BasicAuthControl> logger,
            [FromServices] IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _env = env;
        }

        public async Task<bool> AddNewUserWithCookies(string username, string email, string password)
        {
            bool hasPassed = true;
            _logger.LogInformation("New register request detected");
            var newUser = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var resultCreation = await _userManager.CreateAsync(newUser, password);
            if (resultCreation.Succeeded)
            {
                _logger.LogInformation("Identity user created");
                var claims = new Claim[]
                {
                    new Claim("Bio", "Nothing here at the moment!"),
                    new Claim("Age", "0"),
                    new Claim("CreationDate", $"{DateTime.UtcNow}"),
                    new Claim("PFPath", Path.Combine(_env.WebRootPath, "PFPs", "placeholder.png")),
                    new Claim("ViolatinsCount", "0")
                };
                var resultClaims = await _userManager.AddClaimsAsync(newUser, claims);
                if(resultClaims.Succeeded) { await _signInManager.SignInAsync(newUser, false); }
                else
                {
                    hasPassed = false;
                    _logger.LogWarning("Identity user claims set failed");
                }
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
