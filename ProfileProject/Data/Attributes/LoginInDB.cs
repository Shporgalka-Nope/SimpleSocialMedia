using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Data.Attributes
{
    public class LoginInDB : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {
            ApplicationDbContext dbContext = context.GetService<ApplicationDbContext>();
            string? login = value.ToString().Trim();
            var DBLogins = dbContext.Users.ToList();
            IdentityUser? detectedUser = DBLogins.FirstOrDefault(u => u.UserName == login);

            if(detectedUser != null)
            {
                return new ValidationResult("Someone already has that username!");
            }
            return ValidationResult.Success;
        }
    }
}
