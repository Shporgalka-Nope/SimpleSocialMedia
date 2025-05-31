using Microsoft.AspNetCore.Identity;
using Moq;
using NuGet.ContentModel;
using ProfileProject.Data.Services;
using ProfileProjectTests.Data;
using System.Security.Claims;

namespace ProfileProjectTests;

public class ProfileService_FromIdentity_Tests
{
    [Fact]
    public async void FromIdentity_ReturnsProfile_Null()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(new List<Claim>());

        ProfileService service = new(mockUserManager.Object);

        //Act
        var result = await service.FromIdentity(new IdentityUser("testUser"));

        //Assert
        Assert.Equal("testUser", result.Username);
        Assert.Equal(1, result.Age);
        Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), result.CreationDate);
        Assert.Equal("testUser", result.Username);
        Assert.Equal("Thats my bio!", result.Bio);
        Assert.Equal(Path.Combine("PFPs", "placeholder.png"), result.PFPath);
        Assert.True(result.ShowAge);
        Assert.True(result.ShowInSearch);
        Assert.False(result.IsAllowedToEdit);
    }

    [Fact]
    public async void FromIdentity_ReturnsProfile()
    {
        //Arrange
        List<Claim> claims = new()
        {
            new Claim("Age", "19"),
            new Claim("Bio", "Test bio"),
            new Claim("CreationDate", $"{DateTime.UtcNow}"),
            new Claim("ShowAge", "true"),
            new Claim("ShowInSearch", "true")
        };
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.GetClaimsAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(claims);

        ProfileService service = new(mockUserManager.Object);

        //Act
        var result = await service.FromIdentity(new IdentityUser("testUser"));

        //Assert
        Assert.Equal("testUser", result.Username);
        Assert.Equal(19, result.Age);
        Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), result.CreationDate);
        Assert.Equal("testUser", result.Username);
        Assert.Equal("Test bio", result.Bio);
        Assert.Equal(Path.Combine("PFPs", "placeholder.png"), result.PFPath);
        Assert.True(result.ShowAge);
        Assert.True(result.ShowInSearch);
        Assert.False(result.IsAllowedToEdit);
    }
}
