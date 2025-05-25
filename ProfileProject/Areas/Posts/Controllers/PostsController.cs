using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Data.Services;
using ProfileProject.Models;

namespace ProfileProject.Areas.Posts.Controllers
{
    [Area("Posts")]
    [Route("posts/")]
    public class PostsController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("create/")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("create/")]
        public async Task<IActionResult> Create([FromForm] PostViewModel input, [FromServices] PostService postService)
        {
            if(ModelState.IsValid)
            {
                await postService.CreateNew(input.Title, input.Text, User.Identity.Name);
                return RedirectToAction($"{User.Identity.Name}", "profile");
            }
            return View();
        }
    }
}
