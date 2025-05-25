using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Data.Attributes
{
    public class NoChars : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value = null, ValidationContext context = null)
        {
            char[] bannedChars = new char[] { ',', '.', '|', '/', '*', '=', '+', '!' };
            foreach(char charachter in value.ToString().ToUpperInvariant().Trim())
            {
                if(bannedChars.Contains(charachter))
                {
                    return new ValidationResult($"Character \"{charachter}\" is forbidden");
                }
            }
            return ValidationResult.Success;
        }
    }
}
