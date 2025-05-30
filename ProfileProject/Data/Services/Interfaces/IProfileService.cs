using ProfileProject.Models;

namespace ProfileProject.Data.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetByUsername(string username);
        
        Task<EditViewModel> EditViewModelFromProfile(ProfileViewModel profileVM);

        Task<List<ProfileViewModel?>> GetWithOffset(int offset, int limit);

        Task<List<ProfileViewModel>> ListFromUsername(string username);
        //TO-DO: add all methods
    }
}
