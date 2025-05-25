using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.AppConfig;
using ProfileProject.Models;
using System.Collections.Generic;

namespace ProfileProject.Data.Services
{
    public class PostService
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        public PostService(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task CreateNew(string title, string? text, string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);
            PostModel newPost = new()
            {
                Title = title,
                Text = text,
                Author = user
            };

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();
        }

        public PostModel? GetById(string id)
        {

            var post = _context.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == Guid.Parse(id));
            return post;
        }

        public async Task<List<PostModel>> GetWithOffset(string username, int offset, int limit)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);
            List<PostModel> posts = new();
            if (user != null)
            {
                posts = _context.Posts.Where(p => p.Author == user).OrderByDescending(p => p.CreationDateTime)
                    .Skip(offset).Take(limit).ToList();
            }
            return posts;
        }

        public async Task DeletePost(string id)
        {
            _context.Posts.Remove(_context.Posts.FirstOrDefault(p => p.Id == Guid.Parse(id)));
            await _context.SaveChangesAsync();
        }
    }
}
