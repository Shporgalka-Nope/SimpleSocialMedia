using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ProfileProjectTests;

public class ProfileController_GetByUsername_Tests
{
    [Fact]
    public async void GetByUsername_Get_ReturnsViewWithModel()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success());

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext};

        var mockIProfileService = new Mock<IProfileService>();
        ProfileViewModel profileModel = new();
        mockIProfileService.Setup(m => m.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(profileModel);

        //Act
        var result = await controller.GetByUsername("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<ViewResult>(result);
        Assert.Equal(profileModel, ((ViewResult)result).Model);
        ViewResult viewResult = (ViewResult)result;
        Assert.True(((ProfileViewModel)viewResult.Model).IsAllowedToEdit);
    }

    [Fact]
    public async void GetByUsername_Get_ReturnsNotFound()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new(mockIAuthControl.Object);

        var mockIProfileService = new Mock<IProfileService>();
        mockIProfileService.Setup(m => m.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync((ProfileViewModel)null);

        //Act
        var result = await controller.GetByUsername("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetByUsername_Get_ReturnsViewWithModel_NoOwner()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Failed());

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext };

        var mockIProfileService = new Mock<IProfileService>();
        ProfileViewModel profileModel = new();
        mockIProfileService.Setup(m => m.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(profileModel);

        //Act
        var result = await controller.GetByUsername("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<ViewResult>(result);
        Assert.Equal(profileModel, ((ViewResult)result).Model);
        ViewResult viewResult = (ViewResult)result;
        Assert.False(((ProfileViewModel)viewResult.Model).IsAllowedToEdit);
    }
}
