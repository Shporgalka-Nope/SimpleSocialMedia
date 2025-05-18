using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Models;

namespace ProfileProject.Data.Requirements
{
    public class ProfileOwnerAuthorizationHandler : AuthorizationHandler<ProfileOwnerRequirement, ProfileViewModel>
    {
        private UserManager<IdentityUser> _userManager;
        public ProfileOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ProfileOwnerRequirement requirement, ProfileViewModel profile)
        {
            if(await _userManager.GetUserAsync(context.User) == null)
            {
                return;
            }
            if(context.User.Identity.Name == profile.Username)
            {
                context.Succeed(requirement);
            }
        }
    }
}
