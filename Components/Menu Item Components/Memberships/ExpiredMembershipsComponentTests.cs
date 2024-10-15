using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class ExpiredMembershipsComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IExpiredMembershipsExportService> _mockExportService;
    private readonly Mock<IExpiredMembershipsGetTitleService> _mockTitleService;
    private readonly Mock<IRetieveExpiredMembershipsDataService> _mockDataService;
    private readonly ExpiredMembershipsComponent _component;

    public ExpiredMembershipsComponentTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockExportService = new Mock<IExpiredMembershipsExportService>();
        _mockTitleService = new Mock<IExpiredMembershipsGetTitleService>();
        _mockDataService = new Mock<IRetieveExpiredMembershipsDataService>();

        _component = new ExpiredMembershipsComponent
        {
            Show = _mockAlertService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            ExpiredMembershipsExportService = _mockExportService.Object,
            ExpiredMembershipGetTitleService = _mockTitleService.Object,
            RetrieveExpiredMembershipsDataService = _mockDataService.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_ShouldCallLoadMembersAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadMembersAndManageUICalled = false;
    //    _component.GetType().GetMethod("LoadMembersAndManageUIAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //        ?.Invoke(_component, new object[] { });

    //    // Act
    //    await _component.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadMembersAndManageUICalled);
    //}

    [Fact]
    public async Task LoadMembersAndManageUIAsync_ShouldSetLoadingBDPToFalse()
    {
        // Arrange
        _mockDataService.Setup(s => s.RetrieveExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<ExpiredMembershipsEntity>());
        _mockTitleService.Setup(s => s.GetTitle(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns("Title");

        // Act
        await _component.LoadMembersAndManageUIAsync();

        // Assert
        _mockLoadingPanelService.Verify(s => s.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.LoadingBDP);
    }

    [Fact]
    public async Task LoadMembersAndManageUIAsync_ShouldLogResultOnSuccess()
    {
        // Arrange
        _mockDataService.Setup(s => s.RetrieveExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<ExpiredMembershipsEntity>());
        _mockTitleService.Setup(s => s.GetTitle(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns("Title");

        // Act
        await _component.LoadMembersAndManageUIAsync();

        // Assert
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoadMembersAndManageUIAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockDataService.Setup(s => s.RetrieveExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ThrowsAsync(exception);

        // Act
        await _component.LoadMembersAndManageUIAsync();

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, "ExpiredMembershipsComponent.LoadMembersAndManageUIAsync"), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallExportDataAsync()
    {
        // Arrange
        var fileName = "test.xlsx";
        _component.ExpiredMembershipsEntitiesBDP = new List<ExpiredMembershipsEntity>();
        _component.ExcelGrid = new SfGrid<ExpiredMembershipsEntity>();

        // Act
        await _component.OnClickExportSpreadsheetDatAsync(fileName);

        // Assert
        _mockExportService.Verify(s => s.ExportDataAsync(fileName, _component.ExpiredMembershipsEntitiesBDP, _component.StartDate, _component.EndDate, _component.ExcelGrid), Times.Once);
    }
}