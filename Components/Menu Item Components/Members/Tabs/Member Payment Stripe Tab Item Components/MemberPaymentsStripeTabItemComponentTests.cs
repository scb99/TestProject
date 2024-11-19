using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace MenuItemComponents;

public class MemberPaymentsStripeTabItemComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;
    private readonly Mock<IWebHostEnvironment> _mockHostingEnv;
    private readonly MemberPaymentsStripeTabItemComponent _component;

    public MemberPaymentsStripeTabItemComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockStripeService = new Mock<ICrossCuttingStripeService>();
        _mockHostingEnv = new Mock<IWebHostEnvironment>();

        _component = new MemberPaymentsStripeTabItemComponent();

        _component.SetPrivatePropertyValue("Show", _mockShow.Object);
        _component.SetPrivatePropertyValue("AllMembersInDBService", _mockAllMembersInDBService.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLoggerService.Object);
        _component.SetPrivatePropertyValue("StripeService", _mockStripeService.Object);
        _component.SetPrivatePropertyValue("HostingEnv", _mockHostingEnv.Object);
    }

    //[Fact]
    //public async Task OnChangeAsync_UploadsIIFFile_ShowsSuccessAlert()
    //{
    //    // Arrange
    //    var fileMock = new Mock<IBrowserFile>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("test.iif");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<IBrowserFile> { fileMock.Object }
    //    };

    //    _mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

    //    // Act
    //    await _component.OnChangeAsync(args);

    //    // Assert
    //    _mockShow.Verify(s => s.AlertUsingFallingMessageBoxAsync("Successfully uploaded test.iif!"), Times.Once);
    //}

    //[Fact]
    //public async Task OnChangeAsync_UploadsNonIIFFile_DoesNotShowAlert()
    //{
    //    // Arrange
    //    var fileMock = new Mock<IBrowserFile>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("test.txt");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<IBrowserFile> { fileMock.Object }
    //    };

    //    // Act
    //    await _component.OnChangeAsync(args);

    //    // Assert
    //    _mockShow.Verify(s => s.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Never);
    //}

    [Fact]
    public async Task OnProcessIIFFileClickAsync_ShouldShowAlertIfPathIsEmpty()
    {
        // Arrange

        // Act
        await _component.OnProcessIIFFileClickAsync();

        // Assert
        _mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("Please perform Step 1!"), Times.Once);
    }

    [Fact]
    public async Task OnProcessIIFFileClickAsync_NoIIFFile_ShowsAlert()
    {
        // Arrange
        _component.SetPrivateMemberValue("_path", string.Empty);

        // Act
        await _component.OnProcessIIFFileClickAsync();

        // Assert
        _mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("Please perform Step 1!"), Times.Once);
    }
}