using Microsoft.IdentityModel.Tokens;
using ProfileProject.Data.Services.Interfaces;
using System.Drawing;

namespace ProfileProject.Data.Services
{
    public class ImageProcessor : IImageProcessor
    {

        private IConfiguration _config;
        public ImageProcessor(IConfiguration config)
        {
            _config = config;
        }

        public bool ValidateImage(IFormFile image)
        {
            int maxSizeBytes = (_config.GetSection("Profile:PFPMaxSize").Get<int?>() ?? 10) * 1024 * 1024;

            if (image.Length <= maxSizeBytes)
            {
                List<string> extensions = _config.GetSection("Profile:AllowedExtensions").Get<List<string>?>() 
                    ?? new List<string> {".png", ".jpeg", ".jpg" };
                string? extention = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!string.IsNullOrEmpty(extention) && extensions.Contains(extention))
                {
                    using (var img = Image.FromStream(image.OpenReadStream()))
                    {
                        if (img.Width == img.Height) 
                        {
                            return true; 
                        }
                    }
                }
            }
            return false;
        }
        
        public async Task<string?> SaveImage(IFormFile image)
        {
            var extention = Path.GetExtension(image.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extention}";
            var uploadFolder = Path.Combine("PFPs", "Users", fileName);

            var dir = Directory.GetCurrentDirectory();

            using (FileStream stream = new(Path.Combine("wwwroot", "Profile", uploadFolder), FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return uploadFolder;
        }
    }
}
