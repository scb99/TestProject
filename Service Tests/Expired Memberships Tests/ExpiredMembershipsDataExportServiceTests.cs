using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace ExpiredMemberships;

public class ExpiredMembershipsDataExportServiceTests
{
    private readonly ExpiredMembershipsDataExportService _service;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<IExpiredMembershipsExportService> _mockExpiredMembershipsExport;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;

    public ExpiredMembershipsDataExportServiceTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExpiredMembershipsExport = new Mock<IExpiredMembershipsExportService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();

        _service = new ExpiredMembershipsDataExportService(
            _mockShow.Object,
            _mockExpiredMembershipsExport.Object,
            _mockIsValidFileNameService.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task ExportDataAsync_InvalidFileName_ShowsAlert()
    {
        // Arrange
        var fileName = "invalidFileName";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(false);

        // Act
        await _service.ExportDataAsync(fileName, new List<ExpiredMembershipsEntity>(), DateTime.Now, DateTime.Now, null);

        // Assert
        _mockShow.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
        _mockExpiredMembershipsExport.Verify(service => service.ExportDataAsync(It.IsAny<string>(), It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<ExpiredMembershipsEntity>>()), Times.Never);
    }

    [Fact]
    public async Task ExportDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        var data = new List<ExpiredMembershipsEntity> { new() };
        var startDate = DateTime.Now;
        var endDate = DateTime.Now;
        var grid = new SfGrid<ExpiredMembershipsEntity>();
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);

        // Act
        await _service.ExportDataAsync(fileName, data, startDate, endDate, grid);

        // Assert
        _mockExpiredMembershipsExport.Verify(service => service.ExportDataAsync(fileName, data, startDate, endDate, grid), Times.Once);
    }

    [Fact]
    public async Task ExportDataAsync_ExceptionDuringExport_LogsException()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        var data = new List<ExpiredMembershipsEntity> { new() };
        var startDate = DateTime.Now;
        var endDate = DateTime.Now;
        var grid = new SfGrid<ExpiredMembershipsEntity>();
        var exception = new Exception("Test exception");
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockExpiredMembershipsExport.Setup(service => service.ExportDataAsync(fileName, data, startDate, endDate, grid)).ThrowsAsync(exception);

        // Act
        await _service.ExportDataAsync(fileName, data, startDate, endDate, grid);

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, "DataExportService.ExportDataAsync"), Times.Once);
    }
}