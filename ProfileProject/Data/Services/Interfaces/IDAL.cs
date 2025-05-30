using Microsoft.AspNetCore.Identity;
using ProfileProject.Models;
using System.Collections.Generic;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IDAL
    {
        Task AddPostAsync(PostModel post);

        PostModel? GetPostById(string id);

        List<PostModel?> GetPostsByIdentity(IdentityUser user, int offset, int limit);

        void DeletePostById(string id);

        Task SaveChangesAsync();
    }
}
