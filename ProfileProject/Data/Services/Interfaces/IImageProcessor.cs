namespace ProfileProject.Data.Services.Interfaces
{
    public interface IImageProcessor
    {
        bool ValidateImage(IFormFile image);

        Task<string?> SaveImage(IFormFile image);
    }
}
