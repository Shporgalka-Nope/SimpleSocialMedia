using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Models
{
    public class EditViewModel
    {
        public bool ShowInSearch { get; set; }
        public bool ShowAge { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? NewPFP { get; set; } = null;

        [DataType(DataType.Text)]
        public string? Bio { get; set; }

        [DataType(DataType.Text)]
        public int? Age { get; set; }

        public bool IsAllowedToEdit { get; set; } = false;
    }
}
