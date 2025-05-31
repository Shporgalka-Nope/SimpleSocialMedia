using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProjectTests.Data;
using System.Configuration;
using System.Security.Claims;
using System.Text;

namespace ProfileProjectTests;

public class BasicAuthControl_AddAdditionalInfo_Tests
{
    [Fact]
    public async void AddAdditionalInfo_ThrowsNullreference()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();

        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(m => m.User).Returns(principal);


        var mockSignInManager = new Mock<FakeSignInManager>(mockHttpContext.Object);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => await service.AddAdditionalInfo());
    }

    [Fact]
    public async void AddAdditionalInfo_NewClaimsAreEmpty()
    {
        //Arrange
        List<Claim> oldClaims = new()
        {
            new Claim("Bio", "Thats my bio!"),
            new Claim("Age", "19"),
            new Claim("ShowAge", "true"),
            new Claim("ShowInSearch", "true"),
            new Claim("ShowAge", "true"),
            new Claim("PFPath", Path.Combine("profile", "PFPs", "placeholder.png"))
        };

        List<Claim> newClaims = new List<Claim>();

        Claim[] identityClaims = new[]
        {
            new Claim(ClaimTypes.Name, "testUser")
        };
        var identity = new ClaimsIdentity(identityClaims, null, ClaimTypes.Name, null);
        var principal = new ClaimsPrincipal(identity);
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(m => m.User).Returns(principal);

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => new IdentityUser(username));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(oldClaims);
        mockUserManager.Setup(m => m.ReplaceClaimAsync(It.IsAny<IdentityUser>(), It.IsAny<Claim>(), It.IsAny<Claim>()))
            .ReturnsAsync((IdentityUser user, Claim oldClaim, Claim newClaim) =>
            {
                newClaims.Add(newClaim);
                return IdentityResult.Success;
            });

        var mockSignInManager = new Mock<FakeSignInManager>(mockHttpContext.Object);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();
        var mockIImageProcessor = new Mock<IImageProcessor>();
        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        //Act
        await service.AddAdditionalInfo();

        //Assert
        Assert.Empty(newClaims);
    }

    [Fact]
    public async void AddAdditionalInfo_NewClaims()
    {
        //Arrange
        List<Claim> oldClaims = new()
        {
            new Claim("Bio", "Thats my bio!"),
            new Claim("Age", "19"),
            new Claim("ShowAge", "true"),
            new Claim("ShowInSearch", "true"),
            new Claim("ShowAge", "true"),
            new Claim("PFPath", Path.Combine("profile", "PFPs", "placeholder.png"))
        };

        List<Claim> newClaims = new List<Claim>();

        Claim[] identityClaims = new[]
        {
            new Claim(ClaimTypes.Name, "testUser")
        };
        var identity = new ClaimsIdentity(identityClaims, null, ClaimTypes.Name, null);
        var principal = new ClaimsPrincipal(identity);
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(m => m.User).Returns(principal);

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => new IdentityUser(username));
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(oldClaims);
        mockUserManager.Setup(m => m.ReplaceClaimAsync(It.IsAny<IdentityUser>(), It.IsAny<Claim>(), It.IsAny<Claim>()))
            .ReturnsAsync((IdentityUser user, Claim oldClaim, Claim newClaim) =>
            {
                newClaims.Add(newClaim);
                return IdentityResult.Success;
            });

        var mockSignInManager = new Mock<FakeSignInManager>(mockHttpContext.Object);

        var mockIProfileService = new Mock<IProfileService>();
        var mockIPostService = new Mock<IPostService>();

        //var mockIConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        //var mockSection = new Mock<IConfigurationSection>();
        //mockSection.SetupGet(m => m["PFPMaxSize"]).Returns("10");
        //mockIConfiguration.Setup(m => m.GetSection(It.IsAny<string>()))
        //    .Returns(mockSection.Object);

        var mockIImageProcessor = new Mock<IImageProcessor>();
        mockIImageProcessor.Setup(m => m.ValidateImage(It.IsAny<IFormFile>()))
            .Returns(true);
        mockIImageProcessor.Setup(m => m.SaveImage(It.IsAny<IFormFile>()))
            .ReturnsAsync(@"PFPs/Users/testImage.png");

        var mockIAuthorizationService = new Mock<IAuthorizationService>();

        BasicAuthControl service = new(mockUserManager.Object, mockSignInManager.Object, mockIProfileService.Object,
            mockIPostService.Object, mockIImageProcessor.Object, mockIAuthorizationService.Object);

        byte[] bytes = Encoding.UTF8.GetBytes("testFile");
        MemoryStream stream = new MemoryStream(bytes);
        var file = new FormFile(
            baseStream: stream,
            baseStreamOffset: 0,
            length: bytes.Length,
            name: "Data",
            fileName: "testFile.png"
        );

        //Act
        await service.AddAdditionalInfo("new bio", 1, file, false, false);

        //Assert
        Assert.NotEqual(oldClaims, newClaims);
    }
}
