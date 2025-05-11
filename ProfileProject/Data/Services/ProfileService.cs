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
                Username = identity.UserName,
                Age = int.Parse(claims.FirstOrDefault(x => x.Type == "Age").Value),
                CreationDate = DateOnly.FromDateTime(
                    DateTime.Parse(claims.FirstOrDefault(x => x.Type == "CreationDate").Value)),
                Bio = claims.FirstOrDefault(x => x.Type == "Bio").Value,
                PFPath = claims.FirstOrDefault(x => x.Type == "PFPath").Value
            };
            return profile;
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
