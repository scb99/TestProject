using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Syncfusion.Blazor.Inputs;

namespace MenuItemComponents;

public class MemberPaymentsStripeTabItemComponentTests
{
    //[Fact]
    //public async Task OnChangeAsync_ShouldUploadIIFFile()
    //{
    //    // Arrange
    //    var mockShow = new Mock<ICrossCuttingAlertService>();
    //    var mockLogger = new Mock<ICrossCuttingLoggerService>();
    //    var mockHostingEnv = new Mock<IWebHostEnvironment>();
    //    mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

    //    var component = new MemberPaymentsStripeTabItemComponent
    //    {
    //        Show = mockShow.Object,
    //        Logger = mockLogger.Object,
    //        HostingEnv = mockHostingEnv.Object
    //    };

    //    var fileMock = new Mock<UploadFiles>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("test.iif");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    // Act
    //    await component.OnChangeAsync(args);

    //    // Assert
    //    mockShow.Verify(s => s.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Once);
    //}

    //[Fact]
    //public async Task OnChangeAsync_ShouldLogExceptionOnFailure()
    //{
    //    // Arrange
    //    var mockShow = new Mock<ICrossCuttingAlertService>();
    //    var mockLogger = new Mock<ICrossCuttingLoggerService>();
    //    var mockHostingEnv = new Mock<IWebHostEnvironment>();
    //    mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

    //    var component = new MemberPaymentsStripeTabItemComponent
    //    {
    //        Show = mockShow.Object,
    //        Logger = mockLogger.Object,
    //        HostingEnv = mockHostingEnv.Object
    //    };

    //    var fileMock = new Mock<UploadFiles>();
    //    //fileMock.Setup(f => f.FileInfo.Name).Returns("test.iif");
    //    Syncfusion.Blazor.Inputs.UploaderFiles files = new();
    //    files.Files = new List<Syncfusion.Blazor.Inputs.UploaderFiles>().Add("test.iif");
    //    Syncfusion.Blazor.Inputs.FileInfo fileInfo = new()
    //    {
    //        Name = "test.iif"
    //    };
    //    fileMock.Setup(f => f).Returns(fileInfo.Name);
    //    //fileMock.Setup(f => f.Stream).Throws(new Exception("Test Exception"));

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    // Act
    //    await component.OnChangeAsync(args);

    //    // Assert
    //    mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    //}

    //[Fact]
    //public async Task OnProcessIIFFileClickAsync_ShouldProcessIIFFile()
    //{
    //    // Arrange
    //    var mockShow = new Mock<ICrossCuttingAlertService>();
    //    var mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
    //    var mockStripeService = new Mock<ICrossCuttingStripeService>();

    //    var component = new MemberPaymentsStripeTabItemComponent
    //    {
    //        Show = mockShow.Object,
    //        AllMembersInDBService = mockAllMembersInDBService.Object,
    //        StripeService = mockStripeService.Object
    //    };

    //    var testPath = "C:\\TestPath\\test.iif";
    //    var testStripeEntities = new List<StripeEntity>
    //    {
    //        new() { EmailAddress = "test@example.com" }
    //    };

    //    component.GetType().GetField("_path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(component, testPath);

    //    var parseStripeFileMock = new Mock<ParseStripeFile>(testPath);
    //    parseStripeFileMock.Setup(p => p.Parse()).Returns(testStripeEntities);

    //    mockAllMembersInDBService.Setup(s => s.AllMembersInDB).Returns(new List<MemberEntity> { new() { EmailAddress = "test@example.com", ID = 1, FirstName = "Test", LastName = "User" } });

    //    // Act
    //    await component.OnProcessIIFFileClickAsync();

    //    // Assert
    //    mockStripeService.VerifySet(s => s.StripeEntities = It.IsAny<List<StripeEntity>>(), Times.Once);
    //}

    [Fact]
    public async Task OnProcessIIFFileClickAsync_ShouldShowAlertIfPathIsEmpty()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var component = new MemberPaymentsStripeTabItemComponent
        {
            Show = mockShow.Object
        };

        // Act
        await component.OnProcessIIFFileClickAsync();

        // Assert
        mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("Please perform Step 1!"), Times.Once);
    }
}