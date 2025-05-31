using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;

namespace ProfileProject.Data.Services
{
    public class DAL : IDAL
    {
        ApplicationDbContext _context;
        public DAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPostAsync(PostModel post) => await _context.Posts.AddAsync(post);

        public void DeletePostById(string id)
        {
            _context.Posts.Remove(_context.Posts.FirstOrDefault(p => p.Id == Guid.Parse(id)));
            return;
        }

        public PostModel? GetPostById(string id)
        {
            return _context.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == Guid.Parse(id));
        }

        public List<PostModel?> GetPostsByIdentity(IdentityUser user, int offset, int limit)
        {
            return _context.Posts.Where(p => p.Author == user).OrderByDescending(p => p.CreationDateTime)
                      .Skip(offset).Take(limit).ToList();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();        
    }
}
