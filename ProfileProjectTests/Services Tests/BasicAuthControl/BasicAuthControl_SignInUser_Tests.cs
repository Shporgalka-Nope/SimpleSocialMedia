using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Security.Claims;
using System.Text;

namespace ProfileProjectTests;

public class BasicAuthControl_SignInUser_Tests
{
    [Fact]
    public async void SignInUser_ReturnsNull_NoUser()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.SignInUser("testemail@email.com", "123456789", false);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async void SignInUser_ReturnsNull_SignInFail()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => new IdentityUser(username));

        var mockSignInManager = new Mock<FakeSignInManager>();
        mockSignInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Failed);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.SignInUser("testemail@email.com", "123456789", false);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async void SignInUser_ReturnsProfileViewModel()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => new IdentityUser(username));

        var mockSignInManager = new Mock<FakeSignInManager>();
        mockSignInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        var mockIProfileService = new Mock<IProfileService>();
        mockIProfileService.Setup(m => m.FromIdentity(It.IsAny<IdentityUser>()))
            .ReturnsAsync( new ProfileViewModel() { Username = "testemail@email.com" } );

        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.SignInUser("testemail@email.com", "123456789", false);

        //Assert
        Assert.IsAssignableFrom<ProfileViewModel>(result);
        Assert.Equal("testemail@email.com", result.Username);
    }
}
