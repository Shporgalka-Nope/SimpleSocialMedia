using Microsoft.AspNetCore.Mvc;
using ProfileProject.Areas.Main.Controllers;

namespace ProfileProjectTests;

public class MainController_Tests
{
    [Fact]
    public void Index_notNull()
    {
        //Arrange
        MainController controller = new();

        //Act
        IActionResult? result = controller.Index();

        //Assert
        Assert.NotNull(result);
    }
}
