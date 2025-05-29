using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace ProfileProjectTests;

public class ProfileController_RegisterAddInfo_Tests
{
    [Fact]
    public void RegisterAddInfo_Get_ReturnsView()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        //Act
        var result = controller.RegisterAddInfo();

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void RegisterAddInfo_Post_ReturnsRedirect()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.AddAdditionalInfo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IFormFile>(),
            null, null)).Returns(Task.CompletedTask);

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };
        ProfileController controller = new ProfileController(mockIAuthControl.Object)
        { ControllerContext = testControllerContext };

        RegisterAddInfoViewModel model = new()
        {
            Age = 19,
            Bio = "Test Bio"
        };

        //Act
        var result = await controller.RegisterAddInfo(model);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("profile", ((RedirectToActionResult)result).ControllerName);
        Assert.Equal(testIdentity.Name, ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void RegisterAddInfo_Post_ReturnsViewResult()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);
        controller.ModelState.AddModelError("Error", "Any error");

        RegisterAddInfoViewModel model = new()
        {
            Age = 19,
            Bio = "Test Bio"
        };

        //Act
        var result = await controller.RegisterAddInfo(model);

        //Assert
        Assert.IsType<ViewResult>(result);
    }
}
