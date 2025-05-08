using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProfileProject.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(25, ErrorMessage = "Name is too long")]
        public string Nickname { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Display(Name = "Repeat password")]
        [Required(ErrorMessage = "This field is required")]
        [Compare(nameof(Password), ErrorMessage = "Must match password field")]
        public string RepeatPassword { get; set; }
    }
}
