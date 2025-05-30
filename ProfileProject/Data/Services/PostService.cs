using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.AppConfig;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Collections.Generic;

namespace ProfileProject.Data.Services
{
    public class PostService : IPostService
    {
        private UserManager<IdentityUser> _userManager;
        private IDAL _context;
        public PostService(UserManager<IdentityUser> userManager, IDAL context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task CreateNew(string title, string? text, string username)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(username);
            if(user == null || string.IsNullOrWhiteSpace(title)) { throw new NullReferenceException(); }

            if(string.IsNullOrWhiteSpace(text)) { text = null; }
            PostModel newPost = new()
            {
                Title = title,
                Text = text,
                Author = user
            };

            await _context.AddPostAsync(newPost); 
            await _context.SaveChangesAsync();
        }

        public PostModel? GetById(string id)
        {
            return _context.GetPostById(id);
        }

        public async Task<List<PostModel?>> GetWithOffset(string username, int offset, int limit)
        {
            if(offset < 0 || limit < 0) { throw new IndexOutOfRangeException(); }

            IdentityUser? user = await _userManager.FindByNameAsync(username);
            List<PostModel?> posts = new();
            if (user != null)
            {
                posts = _context.GetPostsByIdentity(user, offset, limit);
            }
            return posts;
        }

        public async Task DeletePost(string id)
        {
            _context.DeletePostById(id);
            await _context.SaveChangesAsync();
        }
    }
}
