using ProfileProject.Models;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetByUsername(string username);
        Task<EditViewModel> EditViewModelFromProfile(ProfileViewModel profileVM);
        //TO-DO: add all methods
    }
}
