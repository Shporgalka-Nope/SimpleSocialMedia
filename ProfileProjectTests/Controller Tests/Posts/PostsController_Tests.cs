using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Posts.Controllers;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace ProfileProjectTests;


public class PostsController_Tests
{
    [Fact]
    public void Create_Get_NotNull()
    {
        //Arrange
        PostsController controller = new();

        //Act
        IActionResult? result = controller.Create();

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async void Create_Post_NotNullIsRedirect()
    {
        //Arrange
        GenericIdentity testIdentity = new GenericIdentity("TestIdentity");
        var testUser = new ClaimsPrincipal(testIdentity);
        HttpContext testContext = new DefaultHttpContext() { User = testUser };
        var testControllerContext = new ControllerContext() { HttpContext = testContext };

        PostsController controller = new() { ControllerContext = testControllerContext };
        PostViewModel postModel = new()
        {
            Text = "Text",
            Title = "Title"
        };
        var mockPostService = new Mock<IPostService>();
        mockPostService.Setup(s => s.CreateNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        IActionResult? result = await controller.Create(postModel, mockPostService.Object);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<RedirectToActionResult>(result);
    }
}
