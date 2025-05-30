using Microsoft.AspNetCore.Authorization;
using ProfileProject.Models;
using System.Security.Claims;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IAuthControl
    {
        Task<ProfileViewModel?> SignInUser(string email, string password, bool rememberMe);

        Task<bool> AddNewUserWithCookies(string username, string email, string password);

        Task AddAdditionalInfo(string? bio = null, int? age = null, IFormFile? pfp = null,
            bool? showAge = null, bool? showInSearch = null);

        Task<AuthorizationResult?> ProveUserOwnership(ClaimsPrincipal loggedUser, string profileUsername);

        Task<AuthorizationResult> ProvePostOwnership(ClaimsPrincipal loggedUser, string postId);
        //TO-DO add all methods
    }
}
