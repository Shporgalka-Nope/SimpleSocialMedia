using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Data.Attributes
{
    public class LoginInDB : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value = null, ValidationContext context = null)
        {
            string login = value.ToString().Trim().ToUpperInvariant();

            ApplicationDbContext dbContext = context.GetService<ApplicationDbContext>();
            var userList = dbContext.Users.ToList();
            IdentityUser? detectedUser = userList.FirstOrDefault(u => u.NormalizedUserName == login);

            if(detectedUser != null) { return new ValidationResult("Someone already has that username!"); }
            return ValidationResult.Success;
        }
    }
}
