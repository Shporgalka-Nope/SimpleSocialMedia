using Microsoft.AspNetCore.Identity;
using Moq;
using NuGet.Protocol;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using ProfileProjectTests.Data;

namespace ProfileProjectTests;

public class PostService_GetById_Tests
{
    [Fact]
    public void GetById_ReturnsPostModel()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        
        var mockIDAL = new Mock<IDAL>();
        mockIDAL.Setup(m => m.GetPostById(It.IsAny<string>()))
            .Returns(new PostModel());

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        //Act
        var result = service.GetById("testId");

        //Assert
        Assert.IsType<PostModel>(result);
    }

    [Fact]
    public void GetById_ReturnsNull()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();

        var mockIDAL = new Mock<IDAL>();
        mockIDAL.Setup(m => m.GetPostById(It.IsAny<string>()))
            .Returns((PostModel)null);

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        //Act
        var result = service.GetById("testId");

        //Assert
        Assert.Null(result);
    }
}
