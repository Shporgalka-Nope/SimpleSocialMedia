using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;

namespace ProfileProjectTests;

public class ProfileController_Register_Tests
{
    [Fact]
    public void Register_Get_ReturnsView()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        //Act
        var result = controller.Register();

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void Register_Post_IsRedirect()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.AddNewUserWithCookies(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        RegisterViewModel model = new()
        {
            Username = "TestUsername",
            Email = "test@email.com",
            Password = "12345678",
            RepeatPassword = "12345678"
        };

        //Act
        var result = await controller.Register(model);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("profile", ((RedirectToActionResult)result).ControllerName);
        Assert.Equal("info", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void Register_Post_InvalidModelState()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);
        controller.ModelState.AddModelError("RepeatPassword", "Not equal");

        RegisterViewModel model = new()
        {
            Username = "TestUsername",
            Email = "test@email.com",
            Password = "12345678",
            RepeatPassword = "ABOBA"
        };

        //Act
        var result = await controller.Register(model);

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void Register_Post_FailedToAdd()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.AddNewUserWithCookies(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        RegisterViewModel model = new()
        {
            Username = "TestUsername",
            Email = "test@email.com",
            Password = "12345678",
            RepeatPassword = "12345678"
        };

        //Act
        var result = await controller.Register(model);

        //Assert
        Assert.IsType<ViewResult>(result);
        Assert.Equal(model, ((ViewResult)result).Model);
    }
}
