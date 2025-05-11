using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Models;
using System.Security.Claims;

namespace ProfileProject.Data.Services
{
    public class BasicAuthControl
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private ILogger<BasicAuthControl> _logger;
        private ProfileService _profileservice;
        public BasicAuthControl(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] ILogger<BasicAuthControl> logger,
            [FromServices] ProfileService profileservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _profileservice = profileservice;
        }

        public async Task<bool> AddNewUserWithCookies(string username, string email, string password)
        {
            var newUser = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var resultCreation = await _userManager.CreateAsync(newUser, password);
            if (resultCreation.Succeeded)
            {
                var claims = new Claim[]
                {
                    new Claim("Bio", "Nothing here at the moment!"),
                    new Claim("Age", "0"),
                    new Claim("CreationDate", $"{DateTime.UtcNow}"),
                    new Claim("PFPath", Path.Combine("PFPs", "placeholder.png")),
                    new Claim("ViolatinsCount", "0")
                };
                var resultClaims = await _userManager.AddClaimsAsync(newUser, claims);
                if(resultClaims.Succeeded) 
                { 
                    await _signInManager.SignInAsync(newUser, false);
                    return true;
                }
            }
            return false;
        }

        public async Task<ProfileViewModel?> SignInUser(string email, string password, bool rememberMe)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, false);
                if(result.Succeeded)
                {
                    ProfileViewModel profileModel = await _profileservice.FromIdentity(user);
                    return profileModel;
                }
            }
            return null;
        }
    }
}
