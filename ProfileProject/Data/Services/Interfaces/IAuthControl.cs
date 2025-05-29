using Microsoft.AspNetCore.Authorization;
using ProfileProject.Models;
using System.Security.Claims;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IAuthControl
    {
        /// <summary>
        /// Email is used to find IdentityUser, for later use in SignInManager with password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        Task<ProfileViewModel?> SignInUser(string email, string password, bool rememberMe);
        Task<bool> AddNewUserWithCookies(string username, string email, string password);
        Task AddAdditionalInfo(string? bio = null, int? age = null, IFormFile? pfp = null,
            bool? showAge = null, bool? showInSearch = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedUser"></param>
        /// <param name="profileUsername"></param>
        /// <returns>Returns true if logged user is an owner, false if not or profile doesnt exist</returns>
        Task<AuthorizationResult?> ProveUserOwnership(ClaimsPrincipal loggedUser, string profileUsername);
        //TO-DO add all methods
    }
}
