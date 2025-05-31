using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProjectTests.Data;

namespace ProfileProjectTests;

public class ProfileController_Logout_Tests
{
    [Fact]
    public async void Logout_Get_ReturnsRedirect()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new(mockIAuthControl.Object);

        var mockSignInManager = new Mock<FakeSignInManager>();
        mockSignInManager.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);

        //Act
        var result = await controller.LogOut(mockSignInManager.Object);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("profile", ((RedirectToActionResult)result).ControllerName);
        Assert.Equal("signin", ((RedirectToActionResult)result).ActionName);
    }
}

