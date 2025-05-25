using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Models
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must have between 1 and 100 characters")]
        [Display(Name = "Post title")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        
        [DataType(DataType.MultilineText)]
        [MaxLength(1500, ErrorMessage = "Limit 1500 characters")]
        public string? Text { get; set; }
    }
}
