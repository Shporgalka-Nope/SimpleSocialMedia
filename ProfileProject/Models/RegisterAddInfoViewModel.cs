using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Models
{
    public class RegisterAddInfoViewModel
    {
        [DataType(DataType.Text)]
        public int? Age { get; set; } = null;

        [DataType(DataType.Text)]
        public string? Bio { get; set; } = null;

        [DataType(DataType.Upload)]
        public IFormFile? PFPath { get; set; } = null;
    }
}
