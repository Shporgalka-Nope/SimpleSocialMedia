using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace ProfileProjectTests;

public class ProfileController_Edit_Tests
{
    [Fact]
    public async void Edit_Get_ReturnsViewWithModel()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success());

        ProfileViewModel model = new();
        EditViewModel editModel = new();
        var mockIProfileService = new Mock<IProfileService>();
        mockIProfileService.Setup(m => m.GetByUsername(It.IsAny<string>()))
            .ReturnsAsync(model);
        mockIProfileService.Setup(m => m.EditViewModelFromProfile(It.IsAny<ProfileViewModel>()))
            .Returns(editModel);

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };
        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext };

        //Act
        var result = await controller.Edit("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<ViewResult>(result);
        Assert.Equal(editModel, ((ViewResult)result).Model);
        ViewResult viewResult = (ViewResult)result;
        EditViewModel viewModel = (EditViewModel)viewResult.Model;
        Assert.True(viewModel.IsAllowedToEdit);
    }

    [Fact]
    public async void Edit_Get_ReturnsForbidden()
    {
        //Arrange
        var mockIProfileService = new Mock<IProfileService>();
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Failed());

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };
        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext };

        //Act
        var result = await controller.Edit("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void Edit_Get_ReturnsForbidden_Null()
    {
        //Arrange
        var mockIProfileService = new Mock<IProfileService>();
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync((AuthorizationResult)null);

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };
        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext };

        //Act
        var result = await controller.Edit("testUsername", mockIProfileService.Object);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void Edit_Post_ReturnsRedirect()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);
        mockIAuthControl.Setup(m => m.AddAdditionalInfo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IFormFile>(),
            It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };
        ProfileController controller = new ProfileController(mockIAuthControl.Object) { ControllerContext = testControllerContext};

        EditViewModel model = new EditViewModel()
        {
            Age = 19,
            Bio = "testBio",
            IsAllowedToEdit = true,
            ShowAge = true,
            ShowInSearch = true
        };


        //Act
        var result = await controller.Edit("testUsername", model);

        //Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("profile", ((RedirectToActionResult)result).ControllerName);
        Assert.Equal(testIdentity.Name, ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void Edit_Post_ReturnsView()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);
        controller.ModelState.AddModelError("error", "any error");
        EditViewModel model = new();

        //Act
        var result = await controller.Edit("testUsername", model);

        //Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async void Edit_Post_ReturnsForbit()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Failed);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        EditViewModel model = new EditViewModel()
        {
            Age = 19,
            Bio = "testBio",
            IsAllowedToEdit = true,
            ShowAge = true,
            ShowInSearch = true
        };

        //Act
        var result = await controller.Edit("testUsername", model);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async void Edit_Post_ReturnsForbit_Null()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        mockIAuthControl.Setup(m => m.ProveUserOwnership(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
            .ReturnsAsync((AuthorizationResult)null);
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        EditViewModel model = new EditViewModel()
        {
            Age = 19,
            Bio = "testBio",
            IsAllowedToEdit = true,
            ShowAge = true,
            ShowInSearch = true
        };

        //Act
        var result = await controller.Edit("testUsername", model);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }
}
