using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace CrossCuttingConcerns;

public class ExportExcelFileServiceTests
{
    private readonly Mock<ICrossCuttingAlertAndLogService> mockAlertAndLogService;
    private readonly Mock<ICrossCuttingFileExportService> mockFileExportService;
    private readonly ExportExcelFileService exportExcelFileService;

    public ExportExcelFileServiceTests()
    {
        mockAlertAndLogService = new Mock<ICrossCuttingAlertAndLogService>();
        mockFileExportService = new Mock<ICrossCuttingFileExportService>();

        exportExcelFileService = new ExportExcelFileService(mockAlertAndLogService.Object, mockFileExportService.Object);
    }

    [Fact]
    public async Task DownloadTextDocumentToUsersMachineAsync_Success()
    {
        // Arrange
        var fileName = "test.txt";
        var text = "Sample text";

        mockFileExportService.Setup(service => service.ExportTextFileAsync(fileName, text)).Returns(Task.CompletedTask);
        mockAlertAndLogService.Setup(service => service.AlertAndLogResultAsync(fileName)).Returns(Task.CompletedTask);

        // Act
        var result = await exportExcelFileService.DownloadTextDocumentToUsersMachineAsync<string>(fileName, text);

        // Assert
        Assert.True(result);
        mockFileExportService.Verify(service => service.ExportTextFileAsync(fileName, text), Times.Once);
        mockAlertAndLogService.Verify(service => service.AlertAndLogResultAsync(fileName), Times.Once);
    }

    [Fact]
    public async Task DownloadTextDocumentToUsersMachineAsync_Failure()
    {
        // Arrange
        var fileName = "test.txt";
        var text = "Sample text";

        mockFileExportService.Setup(service => service.ExportTextFileAsync(fileName, text)).ThrowsAsync(new Exception("Export failed"));
        mockAlertAndLogService.Setup(service => service.AlertAndLogErrorAsync(fileName, It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await exportExcelFileService.DownloadTextDocumentToUsersMachineAsync<string>(fileName, text);

        // Assert
        Assert.False(result);
        mockFileExportService.Verify(service => service.ExportTextFileAsync(fileName, text), Times.Once);
        mockAlertAndLogService.Verify(service => service.AlertAndLogErrorAsync(fileName, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DownloadSpreadsheetDocumentToUsersMachineAsync_Success()
    {
        // Arrange
        var fileName = "test.xlsx";
        var list = new List<string> { "Item1", "Item2" };
        var excelGrid = new SfGrid<string>();

        mockFileExportService.Setup(service => service.ExportSpreadSheetAsync(fileName, list, excelGrid)).Returns(Task.CompletedTask);
        mockAlertAndLogService.Setup(service => service.AlertAndLogResultAsync(fileName)).Returns(Task.CompletedTask);

        // Act
        var result = await exportExcelFileService.DownloadSpreadsheetDocumentToUsersMachineAsync(fileName, list, excelGrid);

        // Assert
        Assert.True(result);
        mockFileExportService.Verify(service => service.ExportSpreadSheetAsync(fileName, list, excelGrid), Times.Once);
        mockAlertAndLogService.Verify(service => service.AlertAndLogResultAsync(fileName), Times.Once);
    }

    [Fact]
    public async Task DownloadSpreadsheetDocumentToUsersMachineAsync_Failure()
    {
        // Arrange
        var fileName = "test.xlsx";
        var list = new List<string> { "Item1", "Item2" };
        var excelGrid = new SfGrid<string>();

        mockFileExportService.Setup(service => service.ExportSpreadSheetAsync(fileName, list, excelGrid)).ThrowsAsync(new Exception("Export failed"));
        mockAlertAndLogService.Setup(service => service.AlertAndLogErrorAsync(fileName, It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await exportExcelFileService.DownloadSpreadsheetDocumentToUsersMachineAsync(fileName, list, excelGrid);

        // Assert
        Assert.False(result);
        mockFileExportService.Verify(service => service.ExportSpreadSheetAsync(fileName, list, excelGrid), Times.Once);
        mockAlertAndLogService.Verify(service => service.AlertAndLogErrorAsync(fileName, It.IsAny<string>()), Times.Once);
    }
}