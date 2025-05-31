using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ProfileProject.Data.Requirements;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Drawing;
using System.Security.Claims;

namespace ProfileProject.Data.Services
{
    public class BasicAuthControl : IAuthControl
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IImageProcessor _imageProcessor;
        private IProfileService _profileservice;
        private IAuthorizationService _authService;
        private IPostService _postService;
        public BasicAuthControl(
            [FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] IProfileService profileservice,
            [FromServices] IPostService postService,
            [FromServices] IImageProcessor imageProcessor,
            [FromServices] IAuthorizationService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageProcessor = imageProcessor;
            _profileservice = profileservice;
            _authService = authService;
            _postService = postService;
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
                    new Claim("ViolatinsCount", "0"),
                    new Claim("ShowAge", "true"),
                    new Claim("ShowInSearch", "true")
                };
                var resultClaims = await _userManager.AddClaimsAsync(newUser, claims);
                await _signInManager.SignInAsync(newUser, false);
                return true;
            }
            return false;
        }

        public async Task AddAdditionalInfo(string? bio = null, int? age = null, IFormFile? pfp = null, 
            bool? showAge = null, bool? showInSearch = null)
        {
            string? username = _signInManager.Context.User.Identity?.Name;
            if(username == null) { throw new NullReferenceException(); }
            IdentityUser user = await _userManager.FindByNameAsync(username);
            var claims = await _userManager.GetClaimsAsync(user);

            if (!string.IsNullOrWhiteSpace(bio))
            {
                Claim oldClaim = claims.FirstOrDefault(c => c.Type == "Bio");
                Claim newClaim = new Claim("Bio", bio);
                await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
            }

            if(age != null)
            {
                Claim oldClaim = claims.FirstOrDefault(c => c.Type == "Age");
                Claim newClaim = new Claim("Age", $"{age}");
                await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
            }

            if(showAge != null)
            {
                Claim oldClaim = claims.FirstOrDefault(c => c.Type == "ShowAge");
                Claim newClaim = new Claim("ShowAge", $"{showAge}");
                await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
            }

            if(showInSearch != null)
            {
                Claim oldClaim = claims.FirstOrDefault(c => c.Type == "ShowInSearch");
                Claim newClaim = new Claim("ShowInSearch", $"{showInSearch}");
                await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
            }

            if(pfp != null && pfp.Length != 0)
            {
                bool result = _imageProcessor.ValidateImage(pfp);
                if(result)
                {
                    string? uploadFolder = await _imageProcessor.SaveImage(pfp);
                    Claim oldClaim = claims.FirstOrDefault(c => c.Type == "PFPath");
                    Claim newClaim = new Claim("PFPath", $"{uploadFolder}");
                    await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
                }
            }
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

        public async Task<AuthorizationResult?> ProveUserOwnership(ClaimsPrincipal loggedUser, string profileUsername)
        {
            IdentityUser? profileUser = await _userManager.FindByNameAsync(profileUsername);
            if(profileUser != null)
            {
                var profile = await _profileservice.FromIdentity(profileUser);
                return await _authService.AuthorizeAsync(loggedUser, profile, "IsAllowedToEdit");
            }
            return null;
        }

        public async Task<AuthorizationResult> ProvePostOwnership(ClaimsPrincipal loggedUser, string postId)
        {
            PostModel? post = _postService.GetById(postId);
            if (post != null)
            {
                return await _authService.AuthorizeAsync(loggedUser, post, "IsPostOwner");
            }
            return null;
        }
    }
}
