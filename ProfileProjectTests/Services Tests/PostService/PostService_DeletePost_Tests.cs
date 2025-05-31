using Moq;
using ProfileProject.Data.Services;
using ProfileProject.Data.Services.Interfaces;
using ProfileProjectTests.Data;

namespace ProfileProjectTests;

public class PostService_DeletePost_Tests
{
    [Fact]
    public async void DeletePost_Returns()
    {
        var mockUserManager = new Mock<FakeUserManager>();

        var mockIDAL = new Mock<IDAL>();
        mockIDAL.Setup(m => m.DeletePostById(It.IsAny<string>()));
        mockIDAL.Setup(m => m.SaveChangesAsync()).Returns(Task.CompletedTask);

        PostService service = new(mockUserManager.Object, mockIDAL.Object);

        await service.DeletePost("fakeId");

        mockIDAL.Verify(m => m.DeletePostById("fakeId"), Times.Once);
        mockIDAL.Verify(m => m.SaveChangesAsync(), Times.Once);
    }
}
