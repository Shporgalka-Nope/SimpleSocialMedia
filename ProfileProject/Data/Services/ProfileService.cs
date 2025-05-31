using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Build.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGet.ProjectModel;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProfileProject.Data.Services
{
    public class ProfileService : IProfileService
    {
        private UserManager<IdentityUser> _userManager;
        public ProfileService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileViewModel> FromIdentity(IdentityUser identity)
        {
            var claims = await _userManager.GetClaimsAsync(identity);
            ProfileViewModel profile = new ProfileViewModel()
            {
                //For claim titles refer to BasicAuthControl.AddNewUserWithCookies()
                Username = identity.UserName,

                Age = int.Parse(claims.FirstOrDefault(x => x.Type == "Age")?.Value 
                    ?? "1"),
                Bio = claims.FirstOrDefault(x => x.Type == "Bio")?.Value 
                    ?? "Thats my bio!",
                PFPath = claims.FirstOrDefault(x => x.Type == "PFPath")?.Value 
                    ?? Path.Combine("PFPs", "placeholder.png"),

                CreationDate = DateOnly.FromDateTime(
                    DateTime.Parse(claims.FirstOrDefault(x => x.Type == "CreationDate")?.Value 
                    ?? $"{ DateTime.UtcNow }")),

                ShowAge = bool.Parse(claims.FirstOrDefault(x => x.Type == "ShowAge")?.Value 
                    ?? "true"),
                ShowInSearch = bool.Parse(claims.FirstOrDefault(x => x.Type == "ShowInSearch")?.Value 
                    ?? "true")
            };
            return profile;
        }

        public EditViewModel EditViewModelFromProfile(ProfileViewModel profileVM)
        {
            EditViewModel editVM = new()
            {
                Age = profileVM.Age,
                Bio = profileVM.Bio,
                ShowAge = profileVM.ShowAge,
                ShowInSearch = profileVM.ShowInSearch
            };
            return editVM;
        }

        public async Task<ProfileViewModel?> GetByUsername(string username)
        {
            IdentityUser? selectedUser = await _userManager.FindByNameAsync(username);
            if(selectedUser != null)
            {
                ProfileViewModel profile = await FromIdentity(selectedUser);
                return profile;
            }
            return null;
        }

        public async Task<List<ProfileViewModel?>> GetWithOffset(int offset, int limit)
        {
            List<IdentityUser> users = _userManager.Users.Skip(offset).Take(30).ToList();
            List<ProfileViewModel> profiles = new();
            foreach(IdentityUser user in users)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if(bool.Parse(claims.FirstOrDefault(c => c.Type == "ShowInSearch")?.Value ?? "true") == true)
                {
                    profiles.Add(await FromIdentity(user));
                }
            }
            return profiles;
        }

        public async Task<List<ProfileViewModel>> ListFromUsername(string username)
        {
            List<ProfileViewModel> profiles = new();
            IdentityUser? user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if(bool.Parse(claims.FirstOrDefault(c => c.Type == "ShowInSearch")?.Value ?? "true"))
                {
                    profiles.Add(await FromIdentity(user));
                }
            }
            return profiles;
        }
        
    }
}
