namespace ProfileProject.Data.Services.Interfaces
{
    public interface IPostService
    {
        Task CreateNew(string title, string? text, string username);
        //TO-DO include all methods
    }
}
