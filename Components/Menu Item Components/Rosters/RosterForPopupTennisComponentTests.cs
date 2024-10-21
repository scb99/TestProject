using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class RosterForPopupTennisComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IRepositoryRosterForPopUpTennis> _mockPopUpTennisRosterRepository;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly RosterForPopupTennisComponent _component;

    public RosterForPopupTennisComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockPopUpTennisRosterRepository = new Mock<IRepositoryRosterForPopUpTennis>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();

        _component = new RosterForPopupTennisComponent
        {
            Show = _mockShow.Object,
            ExportExcelFileService = _mockExportExcelFileService.Object,
            IsValidFileNameService = _mockIsValidFileNameService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            PopUpTennisRosterRepository = _mockPopUpTennisRosterRepository.Object,
            SystemTimeService = _mockSystemTimeService.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_CallsLoadRosterAndManageUIAsync()
    {
        // Arrange
        var gracePeriod = 5;
        var roster = new List<PopUpTennisEntity> { new() { Name = "Test Member" } };
        _component.Initialize(gracePeriod);
        _mockPopUpTennisRosterRepository.Setup(repo => repo.GetRosterForPopUpTennisAsync(gracePeriod)).ReturnsAsync(roster);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.False(_component.LoadingBDP);
    }

    [Fact]
    public async Task LoadRosterAndManageUIAsync_LoadsRosterAndSetsTitle()
    {
        // Arrange
        var gracePeriod = 5;
        var roster = new List<PopUpTennisEntity> { new() { Name = "Test Member" } };
        _component.Initialize(gracePeriod);
        _mockPopUpTennisRosterRepository.Setup(repo => repo.GetRosterForPopUpTennisAsync(gracePeriod)).ReturnsAsync(roster);

        // Act
        await _component.LoadRosterAndManageUIAsync();

        // Assert
        Assert.Equal(roster, _component.PopUpTennisEntitiesBDP);
        Assert.Equal("1 member on roster for PopUp tennis with grace period of 5 days.", _component.TitleBDP);
        _mockLogger.Verify(logger => logger.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoadRosterAndManageUIAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var gracePeriod = 5;
        var exception = new Exception("Test Exception");
        _component.Initialize(gracePeriod);
        _mockPopUpTennisRosterRepository.Setup(repo => repo.GetRosterForPopUpTennisAsync(gracePeriod)).ThrowsAsync(exception);

        // Act
        await _component.LoadRosterAndManageUIAsync();

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        var gracePeriod = 5;
        var now = DateTime.Now;
        var roster = new List<PopUpTennisEntity> { new() { Name = "Test Member" } };
        _component.Initialize(gracePeriod);
        _component.PopUpTennisEntitiesBDP = roster;
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(now);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync(fileName, It.IsAny<List<PopUpTennisEntity>>(), gracePeriod, _component.ExcelGrid), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_InvalidFileName_ShowsAlert()
    {
        // Arrange
        var fileName = "invalidFileName";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(false);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockShow.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        var exception = new Exception("Test Exception");
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Throws(exception);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}