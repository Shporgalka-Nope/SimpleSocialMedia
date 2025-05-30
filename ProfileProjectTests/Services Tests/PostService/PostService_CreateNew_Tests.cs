using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProfileProject.Data;
using ProfileProject.Data.Services;
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
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using(var context = new ApplicationDbContext(options))
        {
            //EnsureCreated() raises
            //Microsoft.Data.Sqlite.SqliteException : SQLite Error 1: 'near "max": syntax error'.
            context.Database.EnsureCreated();
            context.Users.Add(
                new IdentityUser("testUser")
            );
            context.SaveChanges();
        }

        using(var context = new ApplicationDbContext(options))
        {
            var mockUserManager = new Mock<FakeUserManager>();
            mockUserManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => new IdentityUser(name));

            PostService service = new(mockUserManager.Object, context);
            PostModel post = new()
            {
                Title = "Test title",
                Text = "Test text",
                Author = context.Users.FirstOrDefault(u => u.UserName == "testUser")
            };

            //Act
            await service.CreateNew(post.Title, post.Text, post.Author.UserName);

            //Assert
            List<PostModel> posts = context.Posts.ToList();
            Assert.Single<PostModel>(posts);
            Assert.Equal(post, posts.First());
        }

    }
}
