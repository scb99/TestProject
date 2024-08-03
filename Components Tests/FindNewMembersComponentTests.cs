using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class FindNewMembersComponentTests
{
    private readonly Mock<ICrossCuttingFileNameValidationService> _mockFileNameValidationService;
    private readonly Mock<IFindNewMembersExportService> _mockExportService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly FindNewMembersComponent _component;

    public FindNewMembersComponentTests()
    {
        _mockFileNameValidationService = new Mock<ICrossCuttingFileNameValidationService>();
        _mockExportService = new Mock<IFindNewMembersExportService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();

        _component = new FindNewMembersComponent
        {
            FileNameValidationService = _mockFileNameValidationService.Object,
            FindNewMembersExportService = _mockExportService.Object,
            Logger = _mockLogger.Object,
            StartDate = DateTime.Now.AddDays(-30),
            EndDate = DateTime.Now,
            Now = DateTime.Now,
            ExcelGrid = new SfGrid<NewMemberEntity>(),
            NewMemberEntitiesBDP = new List<NewMemberEntity>()
        };
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_FileNameInvalid_DoesNotCallExportService()
    {
        // Arrange
        var fileName = "invalidFileName";
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(fileName)).ReturnsAsync(false);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(s => s.ExportMembersAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<NewMemberEntity>>(), It.IsAny<List<NewMemberEntity>>()), Times.Never);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_FileNameValid_CallsExportService()
    {
        // Arrange
        var fileName = "validFileName";
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(fileName)).ReturnsAsync(true);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(s => s.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, _component.NewMemberEntitiesBDP), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ExceptionThrown_LogsException()
    {
        // Arrange
        var fileName = "validFileName";
        var exception = new Exception("Test exception");
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(fileName)).ReturnsAsync(true);
        _mockExportService.Setup(s => s.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, _component.NewMemberEntitiesBDP)).ThrowsAsync(exception);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}
