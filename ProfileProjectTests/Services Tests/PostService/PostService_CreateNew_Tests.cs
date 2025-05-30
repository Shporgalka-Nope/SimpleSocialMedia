using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProfileProject.Data;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;
using ProfileProjectTests.Data;
using System.Collections;

namespace ProfileProjectTests;

public class PostService_CreateNew_Tests
{
    [Fact]
    public async void CreateNew_AddsPost()
    {
        //Arrange
        IdentityUser user = new IdentityUser("testUser");
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        List<PostModel> postsLocal = new();
        List<PostModel> postsGlobal = new();

        var mockContext = new Mock<IDAL>();
        mockContext.Setup(m => m.AddPostAsync(It.IsAny<PostModel>()))
            .Returns((PostModel post) =>
            {
                postsLocal.Add(post);
                return Task.CompletedTask;
            });
        mockContext.Setup(m => m.SaveChangesAsync())
            .Returns(() =>
            {
                postsGlobal = postsLocal;
                return Task.CompletedTask;
            });

        PostService service = new(mockUserManager.Object, mockContext.Object);

        PostModel post = new()
        {
            Title = "Test title",
            Text = "Test text",
            Author = user
        };
        
        //Act
        await service.CreateNew(post.Title, post.Text, post.Author.UserName);

        //Assert
        List<PostModel> posts = postsGlobal;
        Assert.Single<PostModel>(posts);
        Assert.Equal(post.Title, posts.First().Title);
        Assert.Equal(post.Text, posts.First().Text);
        Assert.Equal(post.Author, posts.First().Author);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(null)]
    public async void CreateNew_AddsPost_TextIsNull(string? text)
    {
        //Arrange
        IdentityUser user = new IdentityUser("testUser");
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        List<PostModel> postsLocal = new();
        List<PostModel> postsGlobal = new();

        var mockContext = new Mock<IDAL>();
        mockContext.Setup(m => m.AddPostAsync(It.IsAny<PostModel>()))
            .Returns((PostModel post) =>
            {
                postsLocal.Add(post);
                return Task.CompletedTask;
            });
        mockContext.Setup(m => m.SaveChangesAsync())
            .Returns(() =>
            {
                postsGlobal = postsLocal;
                return Task.CompletedTask;
            });

        PostService service = new(mockUserManager.Object, mockContext.Object);

        PostModel post = new()
        {
            Title = "Test title",
            Text = text,
            Author = user
        };

        //Act
        await service.CreateNew(post.Title, post.Text, post.Author.UserName);

        //Assert
        List<PostModel> posts = postsGlobal;
        Assert.Single<PostModel>(posts);
        Assert.Equal(post.Title, posts.First().Title);
        Assert.Null(posts.First().Text);
        Assert.Equal(post.Author, posts.First().Author);
    }

    [Theory]
    [InlineData(" ", "testText", "testUser")]
    [InlineData("testTitle", "testText", " ")]
    public async void CreateNew_RaisesNullReferenceException(string title, string? text, string username)
    {
        //Arrange
        IdentityUser user = new IdentityUser("testUser");
        var mockUserManager = new Mock<FakeUserManager>();
        mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) =>
            {
                if(user.UserName == name) { return user; }
                return null;
            });

        var mockContext = new Mock<IDAL>();

        PostService service = new(mockUserManager.Object, mockContext.Object);

        PostModel post = new()
        {
            Title = title,
            Text = text,
            Author = user
        };

        //Act Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => 
        await service.CreateNew(post.Title, post.Text, username));
    }
}
