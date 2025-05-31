using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Security.Claims;

namespace ProfileProjectTests;

public class ProfileService_GetByUsername_Tests
{
    [Fact]
    public async void GetByUsername_ReturnsNull()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetByUsername("testUsername");

        Assert.Null(result);
    }

    [Fact]
    public async void GetByUsername_ReturnsProfileViewModel()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("testUsername"));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<Claim>());

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetByUsername("testUsername");

        Assert.IsAssignableFrom<ProfileViewModel>(result);
        Assert.Equal("testUsername", result.Username);
    }
}
