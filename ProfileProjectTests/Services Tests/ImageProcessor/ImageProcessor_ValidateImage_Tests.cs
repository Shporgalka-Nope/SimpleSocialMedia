using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using ProfileProject.Data.Services;
using System.Text;

namespace ProfileProjectTests;

public class ImageProcessor_ValidateImage_Tests
{
    [Fact]
    public void ValidateImage_ReturnsNull_Length()
    {
        var mockConfigSection = new Mock<IConfigurationSection>();
        mockConfigSection.SetupGet(m => m["PFPMaxSize"]).Returns("10");

        var mockIConfiguration = new Mock<IConfiguration>();
        mockIConfiguration.Setup(m => m.GetSection("Profile:PFPMaxSize"))
            .Returns(mockConfigSection.Object);

        ImageProcessor service = new(mockIConfiguration.Object);

        byte[] bytes = Encoding.UTF8.GetBytes("test content");

        FormFile file = new FormFile(
            baseStream: new MemoryStream(bytes),
            baseStreamOffset: 0,
            length: long.MaxValue,
            name: "Data",
            fileName: "testFile.png"
        );

        bool result = service.ValidateImage(file);

        Assert.False(result);
    }

    [Fact]
    public void ValidateImage_ReturnsNull_Extension()
    {
        var mockConfigSection = new Mock<IConfigurationSection>();
        mockConfigSection.SetupGet(m => m["PFPMaxSize"]).Returns("10");
        mockConfigSection.SetupGet(m => m["AllowedExtensions"]).Returns((string)null);

        var mockIConfiguration = new Mock<IConfiguration>();
        mockIConfiguration.Setup(m => m.GetSection("Profile:PFPMaxSize"))
            .Returns(mockConfigSection.Object);
        mockIConfiguration.Setup(m => m.GetSection("Profile:AllowedExtensions"))
            .Returns(mockConfigSection.Object);

        ImageProcessor service = new(mockIConfiguration.Object);

        byte[] bytes = Encoding.UTF8.GetBytes("test content");

        FormFile file = new FormFile(
            baseStream: new MemoryStream(bytes),
            baseStreamOffset: 0,
            length: bytes.Length,
            name: "Data",
            fileName: "testFile.txt"
        );

        bool result = service.ValidateImage(file);

        Assert.False(result);
    }
}
