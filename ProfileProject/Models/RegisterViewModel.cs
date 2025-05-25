using ProfileProject.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProfileProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 15 characters")]
        [LoginInDB]
        [NoChars]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [EmailInDB]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        [NoChars]
        public string Password { get; set; }

        [Display(Name = "Repeat password")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords dont match")]
        public string RepeatPassword { get; set; }
    }
}
