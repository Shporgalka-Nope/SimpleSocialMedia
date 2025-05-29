using ProfileProject.Models;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IPostService
    {
        Task CreateNew(string title, string? text, string username);
        PostModel? GetById(string id);
        //TO-DO include all methods
    }
}
