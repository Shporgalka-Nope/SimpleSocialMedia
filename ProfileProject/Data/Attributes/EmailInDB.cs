using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Data.Attributes
{
    public class EmailInDB : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ApplicationDbContext dbContext = validationContext.GetService<ApplicationDbContext>();
            string? email = value.ToString().Trim();
            var users = dbContext.Users.ToList();
            var detectedUser = users.FirstOrDefault(u => u.Email == email);

            if(detectedUser != null)
            {
                return new ValidationResult("This email is already taken!");
            }
            return ValidationResult.Success;
        }
    }
}
