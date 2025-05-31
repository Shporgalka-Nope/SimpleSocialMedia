using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileProject.Areas.Profile.Controllers;
using ProfileProject.Data.Services.Interfaces;

namespace ProfileProjectTests;

public class ProfileController_Search_Tests
{
    [Fact]
    public void Search_Get_ReturnsView()
    {
        //Arrange
        var mockIAuthControl = new Mock<IAuthControl>();
        ProfileController controller = new ProfileController(mockIAuthControl.Object);

        //Act
        var result = controller.Search();

        //Assert
        Assert.IsType<ViewResult>(result);
    }
}
