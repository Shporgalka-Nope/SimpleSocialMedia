using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfileProject.Models;

namespace ProfileProject.Data.Requirements
{
    public class PostOwnerAuthorizationHandler : AuthorizationHandler<PostOwnerRequirement, PostModel>
    {
        private UserManager<IdentityUser> _userManager;
        public PostOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            PostOwnerRequirement requirement, PostModel resource)
        {
            IdentityUser user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                return;
            }
            
            if (resource.Author.Id == user.Id)
            {
                context.Succeed(requirement);
            }
        }
    }
}
