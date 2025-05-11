using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Data.Attributes
{
    public class EmailInDB : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string email = value.ToString().Trim();

            ApplicationDbContext dbContext = validationContext.GetService<ApplicationDbContext>();
            var userList = dbContext.Users.ToList();
            var detectedUser = userList.FirstOrDefault(u => u.Email == email);

            if(detectedUser != null) { return new ValidationResult("This email is already taken!"); }
            return ValidationResult.Success;
        }
    }
}
