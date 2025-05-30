using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.API.Controllers;
using ProfileProject.Data.Services.Interfaces;
using ProfileProject.Models;

namespace ProfileProjectTests;

public class ProfileAPIController_Index_Tests
{
    [Theory]
    [InlineData("Mark")]
    [InlineData("John")]
    [InlineData("Maria")]
    public async void Index_Get_ReturnsPartialViewWithUsername(string username)
    {
        //Arrange
        List<ProfileViewModel> profiles = new List<ProfileViewModel>()
        {
            new ProfileViewModel { Username = "Mark" },
            new ProfileViewModel { Username = "John" },
            new ProfileViewModel { Username = "Maria" }
        };
        ProfileAPIController controller = new();

        var mockProfileService = new Mock<IProfileService>();
        mockProfileService.Setup(m => m.ListFromUsername(It.IsAny<string>()))
            .ReturnsAsync((string name) => 
            {
                var list = new List<ProfileViewModel>();
                list.Add(profiles.FirstOrDefault(p => p.Username == name));
                return list;
            });

        //Act
        var result = await controller.Index(username, 0, 0, mockProfileService.Object);

        //Assert
        Assert.IsType<PartialViewResult>(result);
        List<ProfileViewModel> model = ((PartialViewResult)result).Model as List<ProfileViewModel>;
        Assert.IsAssignableFrom<IEnumerable<ProfileViewModel>>(model);
        Assert.Single<ProfileViewModel>(model);
    }

    [Fact]
    public async void Index_Get_ReturnsPartialViewWithUsername_Null()
    {
        //Arrange
        List<ProfileViewModel> profiles = new List<ProfileViewModel>()
        {
            new ProfileViewModel { Username = "Mark" },
            new ProfileViewModel { Username = "John" },
            new ProfileViewModel { Username = "Maria" }
        };
        ProfileAPIController controller = new();

        var mockProfileService = new Mock<IProfileService>();
        mockProfileService.Setup(m => m.ListFromUsername(It.IsAny<string>()))
            .ReturnsAsync((string name) =>
            {
                var list = new List<ProfileViewModel>();
                var user = profiles.FirstOrDefault(p => p.Username == name);
                if(user != null)
                {
                    list.Add(user);
                }
                return list;
            });

        //Act
        var result = await controller.Index("Fictional username", 0, 0, mockProfileService.Object);

        //Assert
        Assert.IsType<PartialViewResult>(result);
        List<ProfileViewModel> model = ((PartialViewResult)result).Model as List<ProfileViewModel>;
        Assert.Empty(model);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(2, 2)]
    [InlineData(4, 2)]
    public async void Index_Get_ReturnsPartialView(int off, int lim)
    {
        //Arrange
        List<ProfileViewModel> profiles = new List<ProfileViewModel>()
        {
            new ProfileViewModel { Username = "Mark" },
            new ProfileViewModel { Username = "John" },
            new ProfileViewModel { Username = "Maria" },
            new ProfileViewModel { Username = "Lia" },
            new ProfileViewModel { Username = "Alex" },
            new ProfileViewModel { Username = "Mayer" }
        };
        ProfileAPIController controller = new();

        var mockProfileService = new Mock<IProfileService>();
        mockProfileService.Setup(m => m.GetWithOffset(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((int offset, int limit) =>
            {
                return profiles.Skip(offset).Take(limit).ToList();
            });

        //Act
        var result = await controller.Index(null, 0, 0, mockProfileService.Object);

        //Assert
        Assert.IsType<PartialViewResult>(result);
        List<ProfileViewModel> model = ((PartialViewResult)result).Model as List<ProfileViewModel>;
        Assert.IsAssignableFrom<IEnumerable<ProfileViewModel>>(model);
    }
}
