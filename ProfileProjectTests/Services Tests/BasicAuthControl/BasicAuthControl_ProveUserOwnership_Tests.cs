using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Data.Services;
using ProfileProjectTests.Data;
using System.Security.Claims;
using ProfileProject.Models;

namespace ProfileProjectTests;

public class BasicAuthControl_ProveUserOwnership_Tests
{
    [Fact]
    public async void ProveUserOwnership_ReturnsNull_NoUser()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        ClaimsIdentity identity = new("testUser");
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //Act
        var result = await service.ProveUserOwnership(principal, "testUser");

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async void ProveUserOwnership_ReturnsAuthorizationResult()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => new IdentityUser(username));


        var mockIProfileService = new Mock<IProfileService>();
        mockIProfileService.Setup(m => m.FromIdentity(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new ProfileViewModel() { Username = "testUser" });

        var mockIAuthorizationService = new Mock<IAuthorizationService>();
        mockIAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<ProfileViewModel>(),
            It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        ClaimsIdentity identity = new("testUser");
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //Act
        var result = await service.ProveUserOwnership(principal, "testUser");

        //Assert
        Assert.IsAssignableFrom<AuthorizationResult>(result);
        Assert.Equal(AuthorizationResult.Success(), result);
    }
}
