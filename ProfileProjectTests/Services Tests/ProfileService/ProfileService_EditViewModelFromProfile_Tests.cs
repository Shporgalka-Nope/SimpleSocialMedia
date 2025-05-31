using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using ProfileProjectTests.Data;

namespace ProfileProjectTests;

public class ProfileService_EditViewModelFromProfile_Tests
{
    [Fact]
    public void EditViewModelFromProfile_ReturnsEditViewModel()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        ProfileService service = new ProfileService(mockUserManager.Object);

        ProfileViewModel profile = new()
        {
            Age = 19,
            Bio = "Test Bio",
            ShowAge = true,
            ShowInSearch = true
        };

        //Act
        var result = service.EditViewModelFromProfile(profile);

        //Assert
        Assert.IsAssignableFrom<EditViewModel>(result);
        Assert.Equal(profile.Age, result.Age);
        Assert.Equal(profile.Bio, result.Bio);
        Assert.Equal(profile.ShowAge, result.ShowAge);
        Assert.Equal(profile.ShowInSearch, result.ShowInSearch);
    }
}
