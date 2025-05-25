using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.FlowAnalysis;
using NuGet.Protocol.Core.Types;
using ProfileProject.Data;
using ProfileProject.Data.Services;
using ProfileProject.Models;

namespace ProfileProject.Areas.API.Controllers
{
    [Area("API")]
    [Route("api/")]
    public class ProfileAPIController : Controller
    {
        [Route("getprofiles/{username?}")]
        public async Task<IActionResult> Index([FromRoute] string username, [FromQuery] int offset, 
            [FromQuery] int limit, [FromServices] ProfileService profileService)
        {
            if(!string.IsNullOrWhiteSpace(username))
            {
                return PartialView("_profilePartial", await profileService.ListFromUsername(username));
            }
            else
            {
                return PartialView("_profilePartial", await profileService.GetWithOffset(offset, limit));
            }
        }

        [Route("getposts/{username:required}")]
        public async Task<IActionResult> GetPosts([FromRoute] string username, [FromQuery] int offset,
            [FromQuery] int limit, [FromServices] PostService postService)
        {
            return PartialView("_postPartial", await postService.GetWithOffset(username, offset, limit));
        }

        [Route("deletepost")]
        public async Task<IActionResult> DeletePost([FromQuery] string postid, [FromServices] PostService postService,
            [FromServices] BasicAuthControl authControl)
        {
            AuthorizationResult result = await authControl.ProvePostOwnership(User, postid);
            if(result == null || !result.Succeeded) { return Forbid(); }
            await postService.DeletePost(postid);
            return RedirectToAction($"{User.Identity.Name}", "profile");
        }

            
    }
}
