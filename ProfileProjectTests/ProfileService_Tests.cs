using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ProfileProject.Data;
using ProfileProject.Data.Services;
using System.Security.Claims;

namespace ProfileProjectTests;


public class ProfileService_Tests
{
    [Fact]
    public void ProfileService_CanGetByUsername()
    {
        var connection = new SqlConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        List<IdentityUser> userList = new List<IdentityUser>()
        {
            new IdentityUser 
            {
                Id = "00000000-0000-0000-0000-000000000001", 
                UserName = "Alex" 
            },
            new IdentityUser 
            {
                Id = "00000000-0000-0000-0000-000000000002",
                UserName = "James" 
            },
            new IdentityUser 
            {
                Id = "00000000-0000-0000-0000-000000000003",
                UserName = "Denis" 
            }
        };

        List<IdentityUserClaim<string>> claimsList = new List<IdentityUserClaim<string>>()
        {
            new IdentityUserClaim<string>
            {
                UserId = userList[0].Id,
                ClaimType = "Bio",
                ClaimValue = "Bio for user Alex"
            },
            new IdentityUserClaim<string>
            {
                UserId = userList[0].Id,
                ClaimType = "Age",
                ClaimValue = "0"
            },
            new IdentityUserClaim<string>
            {
                UserId = userList[0].Id,
                ClaimType = "CreationDate",
                ClaimValue = DateOnly.FromDateTime(DateTime.Now).ToString()
            }
        };

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Users.AddRange(userList);
            context.UserClaims.AddRange(claimsList);
        }

    }
}
