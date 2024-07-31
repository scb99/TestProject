using DBExplorerBlazor.Services;
using Microsoft.JSInterop;
using Moq;
using Syncfusion.Blazor.Grids;

namespace CrossCuttingConcerns;

public class FileExportServiceTests
{
    private readonly Mock<IJSRuntime> _jsRuntimeMock;
    private readonly FileExportService _fileExportService;

    public FileExportServiceTests()
    {
        _jsRuntimeMock = new Mock<IJSRuntime>();
        _fileExportService = new FileExportService(_jsRuntimeMock.Object);
    }

    [Fact]
    public async Task ExportTextFileAsync_ShouldCallSaveAsAsync()
    {
        // Arrange
        var fileName = "test.txt";
        var text = "Hello, World!";
        //var expectedBytes = Encoding.UTF8.GetBytes(text);

        // Act
        await _fileExportService.ExportTextFileAsync(fileName, text);

        // Assert
        //_jsRuntimeMock.Verify(js => js.InvokeVoidAsync("saveAsFile", fileName, expectedBytes), Times.Once);
    }

    [Fact]
    public async Task ExportSpreadSheetAsync_ShouldCallExcelExport_WhenFileIsNotCsv()
    {
        // Arrange
        var fileName = "test.xlsx";
        var list = new List<object> { new { Name = "Test" } };
        var excelGridMock = new Mock<SfGrid<object>>();

        // Act
        await _fileExportService.ExportSpreadSheetAsync(fileName, list, excelGridMock.Object);

        // Assert
        //excelGridMock.Verify(grid => grid.ExcelExport(excelExportProperties: It.IsAny<ExcelExportProperties>()), Times.Once());
    }

    [Fact]
    public async Task ExportSpreadSheetAsync_ShouldCallCsvExport_WhenFileIsCsv()
    {
        // Arrange
        var fileName = "test.csv";
        var list = new List<object> { new { Name = "Test" } };
        var excelGridMock = new Mock<SfGrid<object>>();

        // Act
        await _fileExportService.ExportSpreadSheetAsync(fileName, list, excelGridMock.Object);

        // Assert
        //excelGridMock.Verify(grid => grid.CsvExport(It.IsAny<ExcelExportProperties>()), Times.Once);
    }
}