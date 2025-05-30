using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.API.Controllers;
using ProfileProject.Data.Services.Interfaces;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace ProfileProjectTests;

public class ProfileAPIController_DeletePost_Tests
{
    [Fact]
    public async void DeletePost_Get_ReturnsRedirect()
    {
        //Arrange
        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        ProfileAPIController controller = new() { ControllerContext = testControllerContext };
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProvePostOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);

        var mockIPostService = new Mock<IPostService>();
        mockIPostService.Setup(m => m.DeletePost(It.IsAny<string>())).Returns(Task.CompletedTask);

        //Act
        var result = await controller.DeletePost("1", mockIPostService.Object, mockIAuthControl.Object);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        var redirectModel = (RedirectToActionResult)result;
        Assert.Equal("profile", redirectModel.ControllerName);
        Assert.Equal(testIdentity.Name, redirectModel.ActionName);
    }

    [Fact]
    public async void DeletePost_Get_ReturnsForbid()
    {
        //Arrange
        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        ProfileAPIController controller = new() { ControllerContext = testControllerContext };

        var mockIPostService = new Mock<IPostService>();
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProvePostOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Failed);

        //Act
        var result = await controller.DeletePost("1", mockIPostService.Object, mockIAuthControl.Object);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void DeletePost_Get_ReturnsForbid_Null()
    {
        //Arrange
        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        ProfileAPIController controller = new() { ControllerContext = testControllerContext };

        var mockIPostService = new Mock<IPostService>();
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProvePostOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync((AuthorizationResult)null);

        //Act
        var result = await controller.DeletePost("1", mockIPostService.Object, mockIAuthControl.Object);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }
}
