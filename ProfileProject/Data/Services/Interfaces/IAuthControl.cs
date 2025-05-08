using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileProject.Models;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IAuthControl
    {
        public Task<bool> AddNewUserWithCookies(string Nickname, string Email, string Password);
    }
}
