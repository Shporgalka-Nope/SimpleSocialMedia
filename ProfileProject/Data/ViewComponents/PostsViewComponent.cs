using Microsoft.AspNetCore.Mvc;

namespace ProfileProject.Data.ViewComponents
{
    public class PostsViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        public PostsViewComponent([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }
        //public IViewComponentResult Invoke(int postNumber)
        //{
        //    int range = 0;
        //    try
        //    {
        //        _context.Posts.OrderBy(x => x.Id)
        //            .Skip(range)
        //            .Take(range + postNumber);
        //    }
        //    catch(IndexOutOfRangeException ex)
        //    {
        //        //return something
        //    }
        //    range += postNumber;
            
        //}


    }
}
