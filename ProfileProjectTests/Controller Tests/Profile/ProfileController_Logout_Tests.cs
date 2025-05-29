using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;

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

#region FakeUserManager and SignInManager

public class FakeUserManager : UserManager<IdentityUser>
{
    public FakeUserManager()
        : base(
              new Mock<IUserStore<IdentityUser>>().Object,
              new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<IdentityUser>>().Object,
              new IUserValidator<IdentityUser>[0],
              new IPasswordValidator<IdentityUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<IdentityUser>>>().Object)
    { }
}

public class FakeSignInManager : SignInManager<IdentityUser>
{
    public FakeSignInManager()
        : base(
              new Mock<FakeUserManager>().Object,
              new HttpContextAccessor(),
              new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
              new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
              new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
              new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
              new Mock<Microsoft.AspNetCore.Identity.IUserConfirmation<IdentityUser>>().Object
              )
    { }
}

#endregion

