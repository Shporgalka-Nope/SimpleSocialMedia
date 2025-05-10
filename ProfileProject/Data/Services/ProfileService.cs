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

        public async Task<ProfileViewModel> GetByUsername(string username)
        {
            var selectedUser = await _userManager.FindByNameAsync(username);
            if(selectedUser != null)
            {
                var Claims = await _userManager.GetClaimsAsync(selectedUser);
                ProfileViewModel profile = new ProfileViewModel()
                {
                    Username = selectedUser.UserName,
                    Bio = Claims?.FirstOrDefault(c => c.Type == "Bio").Value,
                    Age = int.Parse(Claims?.FirstOrDefault(c => c.Type == "Age").Value),
                    CreationDate = DateOnly.FromDateTime(
                        DateTime.Parse(Claims.FirstOrDefault(c => c.Type == "CreationDate").Value)),
                    PFPath = Claims.FirstOrDefault(c => c.Type == "PFPath").Value
                };

                //ProfileViewModel profile = new ProfileViewModel();
                //profile.Username = selectedUser.UserName;
                //profile.Bio = Claims?.FirstOrDefault(c => c.Type == "Bio").Value;
                //profile.Age = int.Parse(Claims?.FirstOrDefault(c => c.Type == "Age").Value);
                //profile.CreationDate = DateOnly.FromDateTime(
                //    DateTime.Parse(Claims.FirstOrDefault(c => c.Type == "CreationDate").Value));
                //profile.PFPath = Claims.FirstOrDefault(c => c.Type == "PFPath").Value;

                return profile;
            }
            return null;
        }
    }
}
