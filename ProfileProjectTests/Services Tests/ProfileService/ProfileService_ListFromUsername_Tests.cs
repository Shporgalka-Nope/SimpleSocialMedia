using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Security.Claims;

namespace ProfileProjectTests;

public class ProfileService_ListFromUsername_Tests
{
    [Fact]
    public async void ListFromUsername_ReturnsEmpty()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        ProfileService service = new(mockUserManager.Object);

        var result = await service.ListFromUsername("testUser");

        Assert.IsAssignableFrom<List<ProfileViewModel>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void ListFromUsername_ReturnsList_NoFitting()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("testUser"));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<Claim>() { new Claim("ShowInSearch", "false") });

        ProfileService service = new(mockUserManager.Object);

        var result = await service.ListFromUsername("testUser");

        Assert.Empty(result);
    }

    [Fact]
    public async void ListFromUsername_ReturnsList_NoClaim()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("testUser"));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<Claim>());

        ProfileService service = new(mockUserManager.Object);

        var result = await service.ListFromUsername("testUser");

        Assert.Single<ProfileViewModel>(result);
        Assert.True(result[0].ShowInSearch);
    }

    [Fact]
    public async void ListFromUsername_ReturnsList()
    {
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new IdentityUser("testUser"));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<Claim>() { new Claim("ShowInSearch", "true") });

        ProfileService service = new(mockUserManager.Object);

        var result = await service.ListFromUsername("testUser");

        Assert.Single<ProfileViewModel>(result);
        Assert.True(result[0].ShowInSearch);
    }
}
