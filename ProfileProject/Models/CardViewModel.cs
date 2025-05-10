using ProfileProject.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProfileProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 30 characters")]
        [LoginInDB]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [EmailInDB]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Display(Name = "Repeat password")]
        [Required(ErrorMessage = "This field is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords dont match")]
        public string RepeatPassword { get; set; }
    }
}
