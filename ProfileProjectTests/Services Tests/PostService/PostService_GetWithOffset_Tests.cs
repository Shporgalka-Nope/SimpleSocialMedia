using Microsoft.AspNetCore.Identity;
using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Runtime.CompilerServices;

namespace ProfileProjectTests;

public class PostService_GetWithOffset_Tests
{
    [Theory]
    [InlineData(-1, 2)]
    [InlineData(0, -2)]
    [InlineData(-1, -2)]
    public async void GetWithOffset_RaisesIndexOutOfRange(int offset, int limit)
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        var mockIDAL = new Mock<IDAL>();

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        //Act Assert
        await Assert.ThrowsAsync<IndexOutOfRangeException>( async () => 
        await service.GetWithOffset("fakeUsername", offset, limit));
    }

    [Fact]
    public async void GetWithOffset_ReturnsEmpty()
    {
        //Arrange
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        var mockIDAL = new Mock<IDAL>();

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        //Act
        var result = await service.GetWithOffset("fakeUsername", 0, 4);

        //Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("User1")]
    [InlineData("User2")]
    [InlineData("User3")]
    public async void GetWithOffset_ReturnsList(string username)
    {
        //Arrange
        List<IdentityUser> users = new()
        {
            new IdentityUser("User1"),
            new IdentityUser("User2"),
            new IdentityUser("User3"),
        };
        List<PostModel> posts = new()
        {
            new PostModel() { Author = users[0] },
            new PostModel() { Author = users[0] },
            new PostModel() { Author = users[0] },

            new PostModel() { Author = users[1] },
            new PostModel() { Author = users[1] }
        };
        Dictionary<string, int> postsByUsers = new()
        {
            {"User1", 3},
            {"User2", 2},
            {"User3", 0},
        };

        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => users.FirstOrDefault(p => p.UserName == username));

        var mockIDAL = new Mock<IDAL>();
        mockIDAL.Setup(m => m.GetPostsByIdentity(It.IsAny<IdentityUser>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((IdentityUser user, int offset, int limit) => posts.Where(p => p.Author == user).ToList());

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        //Act
        var result = await service.GetWithOffset(username, 0, 2);

        //Assert
        Assert.Equal(postsByUsers[username], result.Count);
    }
}
