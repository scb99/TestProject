using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class ExpiredMembershipsComponentTests
{
    private readonly Mock<IExpiredMembershipsLoadDataService> mockLoadDataService = new();
    private readonly Mock<IExpiredMembershipsExportService> mockExportService = new();
    private readonly Mock<ILoadingStateService> mockLoadingStateService = new();
    private readonly Mock<ILoggerService> mockLoggerService = new();
    private readonly Mock<IExpiredMembershipsGetTitleService> mockGetTitleService = new();
    private readonly Mock<IAlertService> mockAlertService = new();

    private readonly ExpiredMembershipsComponent component;

    public ExpiredMembershipsComponentTests()
    {
        var services = new ServiceCollection();
        services.AddScoped(_ => mockLoadDataService.Object);
        services.AddScoped(_ => mockExportService.Object);
        services.AddScoped(_ => mockLoadingStateService.Object);
        services.AddScoped(_ => mockLoggerService.Object);
        services.AddScoped(_ => mockGetTitleService.Object);
        services.AddScoped(_ => mockAlertService.Object);

        var serviceProvider = services.BuildServiceProvider();
        var navigationManager = new Mock<NavigationManager>();

        component = new ExpiredMembershipsComponent
        {
            ExpiredMembershipsLoadDataService = mockLoadDataService.Object,
            ExpiredMembershipsExport = mockExportService.Object,
            LoadingStateService = mockLoadingStateService.Object,
            Logger = mockLoggerService.Object,
            ExpiredMembershipGetTitleService = mockGetTitleService.Object,
            Show = mockAlertService.Object
        };
    }

    [Fact]
    public async Task LoadMembersAndManageUIAsync_LoadsDataAndSetsTitle()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity> { new(), new() };
        mockLoadDataService.Setup(s => s.GetExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expiredMemberships);
        mockGetTitleService.Setup(s => s.GetTitle(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns("Test Title");

        // Act
        await component.LoadMembersAndManageUIAsync();

        // Assert
        mockLoadDataService.Verify(s => s.GetExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        mockGetTitleService.Verify(s => s.GetTitle(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        Assert.Equal("Test Title", component.TitleBDP);
        Assert.Equal(2, component.ExpiredMembershipsEntitiesBDP.Count);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_CallsExportService()
    {
        // Arrange
        string fileName = "test.xlsx";
        component.ExpiredMembershipsEntitiesBDP = new List<ExpiredMembershipsEntity> { new() };

        // Act
        await component.OnClickExportSpreadsheetDatAsync(fileName);

        // Assert
        mockExportService.Verify(s => s.ExportDataAsync(fileName, It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<ExpiredMembershipsEntity>>()), Times.Once);
    }

    [Fact]
    public async Task LoadingState_IsManagedCorrectly()
    {
        // Arrange
        mockLoadDataService.Setup(s => s.GetExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<ExpiredMembershipsEntity>());

        // Act
        await component.LoadMembersAndManageUIAsync();

        // Assert
        mockLoadingStateService.Verify(s => s.ShowLoadingPanel(), Times.Once);
        mockLoadingStateService.Verify(s => s.HideLoadingPanel(), Times.Once);
    }
}