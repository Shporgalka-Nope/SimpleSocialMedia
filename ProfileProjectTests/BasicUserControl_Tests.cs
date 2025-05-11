using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services;

namespace ProfileProjectTests;

public class BasicUserControl_Tests
{
    //[Fact]
    //public async void Test_CanCreate()
    //{
    //    var moqStore = new Mock<IUserStore<IdentityUser>>();
    //    var moqUserManager = new Mock<UserManager<IdentityUser>>(moqStore.Object, null, null, null, null, null, null, null, null);
    //    moqUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
    //        .ReturnsAsync(IdentityResult.Success);
    //    moqUserManager.Setup(x => x.AddClaimsAsync(It.IsAny<IdentityUser>(), 
    //        It.IsAny<IEnumerable<System.Security.Claims.Claim>>()))
    //        .ReturnsAsync(IdentityResult.Success);

    //    var moqSignInManager = new Mock<SignInManager<IdentityUser>>(moqUserManager.Object, 
    //        new Mock<IHttpContextAccessor>().Object, 
    //        new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
    //        new Mock<IOptions<IdentityOptions>>().Object, 
    //        new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
    //        new Mock<IAuthenticationSchemeProvider>().Object,
    //        new Mock<IUserConfirmation<IdentityUser>>().Object);
    //    moqSignInManager.Setup(x => x.SignInAsync(It.IsAny<IdentityUser>(), false, null));

    //    var moqLogger = NullLogger<BasicAuthControl>.Instance;

    //    var moqIHosting = new Mock<IWebHostEnvironment>();
    //    moqIHosting.Setup(x => x.WebRootPath).Returns("test/");

    //    var service = new BasicAuthControl(moqUserManager.Object, 
    //        moqSignInManager.Object, 
    //        moqLogger);

    //    bool result = await service.AddNewUserWithCookies("123", "123", "123");

    //    Assert.True(result);
    //}

    //public async void Test_CanSignIn()
    //{
    //    var moqStore = new Mock<IUserStore<IdentityUser>>();
    //    var moqUserManager = new Mock<UserManager<IdentityUser>>(moqStore.Object, null, null, null, null, null, null, null, null);
    //    moqUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
    //        .ReturnsAsync(IdentityResult.Success);
    //    moqUserManager.Setup(x => x.AddClaimsAsync(It.IsAny<IdentityUser>(),
    //        It.IsAny<IEnumerable<System.Security.Claims.Claim>>()))
    //        .ReturnsAsync(IdentityResult.Success);

    //    var moqSignInManager = new Mock<SignInManager<IdentityUser>>(moqUserManager.Object,
    //        new Mock<IHttpContextAccessor>().Object,
    //        new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
    //        new Mock<IOptions<IdentityOptions>>().Object,
    //        new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
    //        new Mock<IAuthenticationSchemeProvider>().Object,
    //        new Mock<IUserConfirmation<IdentityUser>>().Object);
    //    moqSignInManager.Setup(x => x.SignInAsync(It.IsAny<IdentityUser>(), false, null));

    //    var moqLogger = NullLogger<BasicAuthControl>.Instance;

    //    var moqIHosting = new Mock<IWebHostEnvironment>();
    //}
}
