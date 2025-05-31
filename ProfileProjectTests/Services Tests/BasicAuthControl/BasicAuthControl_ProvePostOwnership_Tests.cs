using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Data.Services;
using ProfileProjectTests.Data;
using System.Security.Claims;
using ProfileProject.Models;

namespace ProfileProjectTests;

public class BasicAuthControl_ProvePostOwnership_Tests
{
    [Fact]
    public async void ProvePostOwnership_ReturnsNull()
    {
        //Arrange
        var mockIPostService = new Mock<IPostService>();
        mockIPostService.Setup(m => m.GetById(It.IsAny<string>()))
            .Returns((PostModel)null);

        var mockIAuthorizationService = new Mock<IAuthorizationService>();
        var mockUserManager = new Mock<FakeUserManager>();
        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIProfileService = new Mock<IProfileService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        ClaimsIdentity identity = new("testUser");
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //Act
        var result = await service.ProvePostOwnership(principal, "testId");

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async void ProvePostOwnership_ReturnsAuthorizationResult()
    {
        //Arrange
        var mockIPostService = new Mock<IPostService>();
        mockIPostService.Setup(m => m.GetById(It.IsAny<string>()))
            .Returns(new PostModel());

        var mockIAuthorizationService = new Mock<IAuthorizationService>();
        mockIAuthorizationService.Setup(m => m.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<PostModel>(),
            It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        var mockUserManager = new Mock<FakeUserManager>();
        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIProfileService = new Mock<IProfileService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        ClaimsIdentity identity = new("testUser");
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //Act
        var result = await service.ProvePostOwnership(principal, "testId");

        //Assert
        Assert.IsAssignableFrom<AuthorizationResult>(result);
    }
}
