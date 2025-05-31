using ProfileProject.Models;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IPostService
    {
        Task CreateNew(string title, string? text, string username);

        PostModel? GetById(string id);

        Task<List<PostModel?>> GetWithOffset(string username, int offset, int limit);

        Task DeletePost(string id);
    }
}
