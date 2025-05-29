using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;

namespace ProfileProjectTests;


public class ProfileController_SignIn_Tests
{
    [Fact]
    public void SignIn_Get_ReturnsView()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        //Act
        var result = controller.SignIn();

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void SignIn_Post_IsRedirect()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileViewModel profileVM = new()
        {
            Username = "TestProfile",
        };
        mockIAuthControl.Setup(m => m.SignInUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(profileVM);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        SignInViewModel signInVM = new()
        {
            Email = "test@email.com",
            Password = "12345678",
            RememberMe = true
        };

        //Act
        var result = await controller.SignIn(signInVM);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("profile", ((RedirectToActionResult)result).ControllerName);
        Assert.Equal($"{profileVM.Username}", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void SignIn_Post_InvalidModelState()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.SignInUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ProfileViewModel)null);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        SignInViewModel signInVM = new()
        {
            Email = "test@email.com",
            Password = "12345678",
            RememberMe = true
        };

        //Act
        var result = await controller.SignIn(signInVM);

        //Assert
        Assert.IsType<ViewResult>(result);
        Assert.Equal(((ViewResult)result).Model, signInVM);
    }

    [Fact]
    public async void SignIn_Post_ModelIsNull()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);
        controller.ModelState.AddModelError("Password", "Required");

        SignInViewModel signInVM = new()
        {
            Email = "test@email.com",
            Password = "12345678",
            RememberMe = true
        };

        //Act
        var result = await controller.SignIn(signInVM);

        //Assert
        Assert.IsType<ViewResult>(result);
    }
}
