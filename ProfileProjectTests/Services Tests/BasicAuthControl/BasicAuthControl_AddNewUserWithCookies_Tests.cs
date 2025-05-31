using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProjectTests.Data;
using System.Security.Claims;

namespace ProfileProjectTests;

public class BasicAuthControl_AddNewUserWithCookies_Tests
{
    [Fact]
    public async void AddNewUserWithCookies_ReturnsFalse_UserCreationFail()
    {
        //Arrange
        List<IdentityUser> registeredUsers = new();

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync((IdentityUser user, string password) =>
            {
                //actual password stored instead of HASH for simplisity purposes
                //user.PasswordHash = password;
                //registeredUsers.Add(user);
                return IdentityResult.Failed();
            });

        var mockSignInManager = new Mock<FakeSignInManager>();
        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object, 
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.AddNewUserWithCookies("testUsername", "testEmail@email.com", "12345678");

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async void AddNewUserWithCookies_ReturnsTrue_ClaimsCreationFail()
    {
        //Arrange
        List<IdentityUser> registeredUsers = new();
        List<Claim> userClaims = new();

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync((IdentityUser user, string password) =>
            {
                //actual password stored instead of HASH for simplisity purposes
                user.PasswordHash = password;
                registeredUsers.Add(user);
                return IdentityResult.Success;
            });
        mockUserManager.Setup(m => m.AddClaimsAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync(IdentityResult.Failed());

        var mockSignInManager = new Mock<FakeSignInManager>();
        mockSignInManager.Setup(m => m.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), null))
            .Returns(Task.CompletedTask);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.AddNewUserWithCookies("testUsername", "testEmail@email.com", "12345678");

        //Assert
        Assert.True(result);
        Assert.Single<IdentityUser>(registeredUsers);
        Assert.Empty(userClaims);
    }

    [Fact]
    public async void AddNewUserWithCookies_ReturnsTrue()
    {
        //Arrange
        List<IdentityUser> registeredUsers = new();
        List<Claim> userClaims = new();

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync((IdentityUser user, string password) =>
            {
                //actual password stored instead of HASH for simplisity purposes
                user.PasswordHash = password;
                registeredUsers.Add(user);
                return IdentityResult.Success;
            });
        mockUserManager.Setup(m => m.AddClaimsAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync((IdentityUser user, Claim[] claims) =>
            {
                foreach(Claim claim in claims)
                {
                    userClaims.Add(claim);
                }
                return IdentityResult.Success;
            });

        var mockSignInManager = new Mock<FakeSignInManager>();
        mockSignInManager.Setup(m => m.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), null))
            .Returns(Task.CompletedTask);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        var result = await service.AddNewUserWithCookies("testUsername", "testEmail@email.com", "12345678");

        //Assert
        Assert.True(result);
        Assert.Single<IdentityUser>(registeredUsers);
        Assert.Equal(7, userClaims.Count);
    }
}
