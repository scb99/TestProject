using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Syncfusion.Blazor.DropDowns;
using VanguardLib;

namespace MenuItemComponents;

public class ProcessVanguardCSVPageTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<IWebHostEnvironment> _mockHostingEnv;
    private readonly ProcessVanguardCSVPage _page;

    public ProcessVanguardCSVPageTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockHostingEnv = new Mock<IWebHostEnvironment>();
        _page = new ProcessVanguardCSVPage
        {
            Show = _mockAlertService.Object,
            Logger = _mockLoggerService.Object,
            HostingEnv = _mockHostingEnv.Object
        };
    }

    [Fact]
    public void OnButtonClick_ResetsState()
    {
        // Arrange
        _page.OnButtonClick();

        // Act
        var itemsField = _page.GetType().GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var itemsValue = itemsField?.GetValue(_page) as List<DropDownListItems>;
        var textAreaValue = _page.GetType().GetProperty("TextAreaValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_page)?.ToString();

        // Assert
        Assert.NotNull(itemsValue);
        Assert.Empty(itemsValue);
        Assert.Equal(string.Empty, textAreaValue);
    }

    //[Fact]
    //public async Task OnChangeAsync_UploadsCSVFileSuccessfully()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    //fileMock.Setup(f => f.FileInfo.Name).Returns("test.csv");
    //    var fileInfoMock = new Mock<Syncfusion.Blazor.Inputs.FileInfo>("test.csv");
    //    //fileMock.Setup(f => f.FileInfo).Returns(fileInfoMock.Object);
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    _mockHostingEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());

    //    // Act
    //    await _page.OnChangeAsync(args);

    //    // Assert
    //    _mockAlertService.Verify(alert => alert.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Once);
    //}

    [Fact]
    public void OnValueChange_UpdatesTextAreaValue()
    {
        // Arrange
        var args = new ChangeEventArgs<string, DropDownListItems>
        {
            ItemData = new DropDownListItems { Text = "Sample Report" }
        };

        // Act
        _page.OnValueChange(args);

        // Assert
        var textAreaValue = _page.GetType().GetProperty("TextAreaValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_page)?.ToString();
        Assert.Equal(ParseVanguardDataFile.GenerateReportToString("Sample Report"), textAreaValue);
    }

    [Fact]
    public void ResetState_ClearsItemsAndTextAreaValue()
    {
        // Act
        _page.ResetState();

        // Assert
        var itemsField = _page.GetType().GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var itemsValue = itemsField?.GetValue(_page) as List<DropDownListItems>;
        var textAreaValue = _page.GetType().GetProperty("TextAreaValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_page)?.ToString();

        Assert.NotNull(itemsValue);
        Assert.Empty(itemsValue);
        Assert.Equal(string.Empty, textAreaValue);
    }
}