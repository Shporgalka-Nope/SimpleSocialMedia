using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Security.Claims;

namespace ProfileProjectTests;

public class ProfileService_GetWithOffset_Tests
{
    [Fact]
    public async void GetWithOffset_ReturnsEmpty()
    {
        var profiles = new List<IdentityUser>().AsQueryable();
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.Users)
            .Returns(profiles);

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetWithOffset(0, 2);

        Assert.Empty(result);
    }

    [Fact]
    public async void GetWithOffset_ReturnsEmpty_NoFitting()
    {
        var profiles = new List<IdentityUser>()
        {
            new IdentityUser("User1"),
            new IdentityUser("User2"),
            new IdentityUser("User3")
        };

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.Users)
            .Returns(profiles.AsQueryable());
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync((IdentityUser user) =>
            {
                return new List<Claim>() { new Claim("ShowInSearch", "false") };
            });

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetWithOffset(0, 2);

        Assert.Empty(result);
    }

    [Fact]
    public async void GetWithOffset_ReturnsListWithModels_NoClaim()
    {
        var profiles = new List<IdentityUser>()
        {
            new IdentityUser("User1"),
            new IdentityUser("User2")
        };

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.Users)
            .Returns(profiles.AsQueryable());
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync((IdentityUser user) =>
            {
                return new List<Claim?>();
            });

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetWithOffset(0, 2);

        Assert.IsAssignableFrom<List<ProfileViewModel>>(result);
        Assert.Equal(2, result.Count);
        Assert.True(result[0].ShowInSearch);
        Assert.True(result[1].ShowInSearch);
    }

    [Fact]
    public async void GetWithOffset_ReturnsListWithModels()
    {
        var profiles = new List<IdentityUser>()
        {
            new IdentityUser("User1"),
            new IdentityUser("User2")
        };

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.Users)
            .Returns(profiles.AsQueryable());
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync((IdentityUser user) =>
            {
                return new List<Claim>() { new Claim("ShowInSearch", "true") };
            });

        ProfileService service = new(mockUserManager.Object);

        var result = await service.GetWithOffset(0, 2);

        Assert.IsAssignableFrom<List<ProfileViewModel>>(result);
        Assert.Equal(2, result.Count);
        Assert.True(result[0].ShowInSearch);
        Assert.True(result[1].ShowInSearch);
    }
}
