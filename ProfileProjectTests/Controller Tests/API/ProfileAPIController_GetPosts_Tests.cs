using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.DependencyResolver;
using ProfileProject.Areas.API.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using System.Threading.Tasks;

namespace ProfileProjectTests;

public class ProfileAPIController_GetPosts_Tests
{
    [Fact]
    public async Task GetPosts_Get_ReturnsPartialView()
    {
        //Arrange
        ProfileAPIController controller = new();

        var mockIPostService = new Mock<IPostService>();
        List<PostModel> posts = new()
        {
            new PostModel(),
            new PostModel()
        };
        mockIPostService.Setup(m => m.GetWithOffset(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(posts);

        //Act
        var result = await controller.GetPosts("testUsername", 0, 2, mockIPostService.Object);

        //Assert
        Assert.IsType<PartialViewResult>(result);
        PartialViewResult viewResult = (PartialViewResult)result;
        Assert.IsAssignableFrom<IEnumerable<PostModel>>(viewResult.Model);
    }
}
