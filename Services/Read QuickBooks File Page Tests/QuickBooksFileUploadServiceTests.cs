using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Syncfusion.Blazor.Inputs;

namespace Services;

public class QuickBooksFileUploadServiceTests
{
    private readonly Mock<IWebHostEnvironment> _mockHostingEnv;
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly QuickBooksFileUploadService _service;

    public QuickBooksFileUploadServiceTests()
    {
        _mockHostingEnv = new Mock<IWebHostEnvironment>();
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();

        _mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

        _service = new QuickBooksFileUploadService(
            _mockHostingEnv.Object,
            _mockAlertService.Object,
            _mockLoggerService.Object
        );
    }

    //[Fact]
    //public async Task UploadFilesAsync_UploadsDuesCsvFile()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("Dues.csv");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    // Act
    //    await _service.UploadFilesAsync(args);

    //    // Assert
    //    _mockShow.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Successfully uploaded Dues.csv!"), Times.Once);
    //}

    //[Fact]
    //public async Task UploadFilesAsync_UploadsDuesPrepaidCsvFile()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("DuesPrepaid.csv");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream());

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    // Act
    //    await _service.UploadFilesAsync(args);

    //    // Assert
    //    _mockShow.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Successfully uploaded DuesPrepaid.csv!"), Times.Once);
    //}

    //[Fact]
    //public async Task UploadFilesAsync_LogsExceptionOnFailure()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    fileMock.Setup(f => f.FileInfo.Name).Returns("Dues.csv");
    //    fileMock.Setup(f => f.Stream).Throws(new Exception("Test Exception"));

    //    var args = new UploadChangeEventArgs
    //    {
    //        Files = new List<UploadFiles> { fileMock.Object }
    //    };

    //    // Act
    //    await _service.UploadFilesAsync(args);

    //    // Assert
    //    _mockLoggerService.Verify(logger => logger.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    //}
}