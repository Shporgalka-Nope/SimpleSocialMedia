using Microsoft.AspNetCore.Identity;
using NuGet.ProjectModel;
using ProfileProject.Models;
using System.Threading.Tasks;

namespace ProfileProject.Data.Services
{
    public class ProfileService
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
                Age = int.Parse(claims.FirstOrDefault(x => x.Type == "Age").Value 
                    ?? "1"),
                CreationDate = DateOnly.FromDateTime(
                    DateTime.Parse(claims.FirstOrDefault(x => x.Type == "CreationDate").Value 
                    ?? $"{ DateTime.Now }")),
                Bio = claims.FirstOrDefault(x => x.Type == "Bio").Value 
                    ?? "Thats my bio!",
                PFPath = claims.FirstOrDefault(x => x.Type == "PFPath").Value 
                    ?? Path.Combine("PFPs", "placeholder.png"),
                ShowAge = bool.Parse(claims.FirstOrDefault(x => x.Type == "ShowAge").Value 
                    ?? "true"),
                ShowInSearch = bool.Parse(claims.FirstOrDefault(x => x.Type == "ShowInSearch").Value 
                    ?? "true")
            };
            return profile;
        }

        public async Task<EditViewModel> EditViewModelFromProfile(ProfileViewModel profileVM)
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
            IdentityUser selectedUser = await _userManager.FindByNameAsync(username);
            if(selectedUser != null)
            {
                ProfileViewModel profile = await FromIdentity(selectedUser);
                return profile;
            }
            return null;
        }
    }
}
