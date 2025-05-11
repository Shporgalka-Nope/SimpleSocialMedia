using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProfileProject.Data.Attributes
{
    public class OnlyAnonymousAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult($"{context.HttpContext.User.Identity.Name}",
                    "profile", new { area = "" }); 
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }

    }
}
