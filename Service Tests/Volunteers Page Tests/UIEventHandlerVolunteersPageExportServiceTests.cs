using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class UIEventHandlerVolunteersPageExportServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly UIEventHandlerVolunteersPageExportService _exportService;

    public UIEventHandlerVolunteersPageExportServiceTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();

        _exportService = new UIEventHandlerVolunteersPageExportService(
            _mockAlertService.Object,
            _mockExportExcelFileService.Object,
            _mockIsValidFileNameService.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task HandleExportAsync_InvalidFileName_ShowsAlert()
    {
        // Arrange
        var fileName = "invalid|name";
        var volunteerLists = new VolunteerLists();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        var excelGrid = new Mock<SfGrid<TypeOfVolunteer>>().Object;

        _mockIsValidFileNameService
            .Setup(service => service.FileNameValid(fileName))
            .Returns(false);

        // Act
        await _exportService.HandleExportAsync(fileName, volunteerLists, volunteersEntities2BDP, excelGrid);

        // Assert
        _mockAlertService.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
        _mockExportExcelFileService.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleExportAsync_ValidTextFileName_DownloadsTextDocument()
    {
        // Arrange
        var fileName = "valid.txt";
        var volunteerLists = new VolunteerLists();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        var excelGrid = new Mock<SfGrid<TypeOfVolunteer>>().Object;

        _mockIsValidFileNameService
            .Setup(service => service.FileNameValid(fileName))
            .Returns(true);

        _mockIsValidFileNameService
            .Setup(service => service.IsFileNameATextFile(fileName))
            .Returns(true);

        // Act
        await _exportService.HandleExportAsync(fileName, volunteerLists, volunteersEntities2BDP, excelGrid);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadTextDocumentToUsersMachineAsync<string>(fileName, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleExportAsync_ValidSpreadsheetFileName_DownloadsSpreadsheetDocument()
    {
        // Arrange
        var fileName = "valid.xlsx";
        var volunteerLists = new VolunteerLists();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        var excelGrid = new Mock<SfGrid<TypeOfVolunteer>>().Object;

        _mockIsValidFileNameService
            .Setup(service => service.FileNameValid(fileName))
            .Returns(true);

        _mockIsValidFileNameService
            .Setup(service => service.IsFileNameATextFile(fileName))
            .Returns(false);

        // Act
        await _exportService.HandleExportAsync(fileName, volunteerLists, volunteersEntities2BDP, excelGrid);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync<TypeOfVolunteer>(fileName, volunteersEntities2BDP, excelGrid), Times.Once);
    }

    [Fact]
    public async Task HandleExportAsync_Exception_LogsException()
    {
        // Arrange
        var fileName = "valid.xlsx";
        var volunteerLists = new VolunteerLists();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        var excelGrid = new Mock<SfGrid<TypeOfVolunteer>>().Object;
        var exception = new Exception("Test exception");

        _mockIsValidFileNameService
            .Setup(service => service.FileNameValid(fileName))
            .Returns(true);

        _mockIsValidFileNameService
            .Setup(service => service.IsFileNameATextFile(fileName))
            .Returns(false);

        _mockExportExcelFileService
            .Setup(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync<TypeOfVolunteer>(fileName, volunteersEntities2BDP, excelGrid))
            .ThrowsAsync(exception);

        // Act
        await _exportService.HandleExportAsync(fileName, volunteerLists, volunteersEntities2BDP, excelGrid);

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}