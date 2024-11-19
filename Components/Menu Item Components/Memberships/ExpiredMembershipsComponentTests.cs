using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
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

        _component = new ExpiredMembershipsComponent();

        _component.SetPrivatePropertyValue("Show", _mockAlertService.Object);
        _component.SetPrivatePropertyValue("LoadingPanelService", _mockLoadingPanelService.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLogger.Object);
        _component.SetPrivatePropertyValue("ExpiredMembershipsExportService", _mockExportService.Object);
        _component.SetPrivatePropertyValue("ExpiredMembershipGetTitleService", _mockTitleService.Object);
        _component.SetPrivatePropertyValue("RetrieveExpiredMembershipsDataService", _mockDataService.Object);
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldCallLoadMembersAndManageUIAsync()
    {
        // Arrange
        var loadMembersAndManageUICalled = true;

        // Act
        await typeof(ExpiredMembershipsComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.True(loadMembersAndManageUICalled);
    }

    [Fact]
    public async Task LoadMembersAndManageUIAsync_ShouldSetLoadingBDPToFalse()
    {
        // Arrange
        _mockDataService.Setup(s => s.RetrieveExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<ExpiredMembershipsEntity>());
        _mockTitleService.Setup(s => s.GetTitle(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns("Title");

        // Act
        await typeof(ExpiredMembershipsComponent).InvokeAsync("LoadMembersAndManageUIAsync", _component);

        // Assert
        _mockLoadingPanelService.Verify(s => s.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
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
        await typeof(ExpiredMembershipsComponent).InvokeAsync("LoadMembersAndManageUIAsync", _component);

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
        await typeof(ExpiredMembershipsComponent).InvokeAsync("LoadMembersAndManageUIAsync", _component);

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, "ExpiredMembershipsComponent.LoadMembersAndManageUIAsync"), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallExportDataAsync()
    {
        // Arrange
        var fileName = "test.xlsx";
        _component.SetPrivatePropertyValue<List<ExpiredMembershipsEntity>>("ExpiredMembershipsEntitiesBDP", new List<ExpiredMembershipsEntity>());
        _component.SetPrivatePropertyValue<SfGrid<ExpiredMembershipsEntity>>("ExcelGrid", new SfGrid<ExpiredMembershipsEntity>());

        // Act
        await typeof(ExpiredMembershipsComponent).InvokeAsync("OnClickExportSpreadsheetDatAsync", _component, fileName);

        // Assert
        _mockExportService.Verify(s => s.ExportDataAsync(fileName, _component.GetPrivatePropertyValue<List<ExpiredMembershipsEntity>>("ExpiredMembershipsEntitiesBDP"), _component.StartDate, _component.EndDate, _component.GetPrivatePropertyValue<SfGrid<ExpiredMembershipsEntity>>("ExcelGrid")), Times.Once);
    }
}