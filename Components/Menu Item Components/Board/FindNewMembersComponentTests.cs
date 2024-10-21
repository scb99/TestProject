using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class FindNewMembersComponentTests
{
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<IFindNewMembersExportService> _mockExportService;
    private readonly Mock<IRetrieveNewMembersDataService> _mockRetrieveService;
    private readonly FindNewMembersComponent _component;

    public FindNewMembersComponentTests()
    {
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockExportService = new Mock<IFindNewMembersExportService>();
        _mockRetrieveService = new Mock<IRetrieveNewMembersDataService>();

        _component = new FindNewMembersComponent
        {
            LoadingPanelService = _mockLoadingPanelService.Object,
            FindNewMembersExportService = _mockExportService.Object,
            RetrieveNewMembersDataService = _mockRetrieveService.Object,
        };

        _component.Initialize(DateTime.Now.AddDays(-30), DateTime.Now);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsNewMemberEntitiesAndTitle()
    {
        // Arrange
        var members = new List<NewMemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveNewMembersDataAsync(_component.StartDate, _component.EndDate))
                            .ReturnsAsync(members);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(members, _component.NewMemberEntitiesBDP);
        Assert.Equal($"{members.Count} new member{(members.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.TitleBDP);
    }

    [Fact]
    public async Task LoadNewMembersAndManageUIAsync_SetsLoadingStateCorrectly()
    {
        // Arrange
        var members = new List<NewMemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveNewMembersDataAsync(_component.StartDate, _component.EndDate))
                            .ReturnsAsync(members);

        // Act
        await _component.LoadNewMembersAndManageUIAsync();

        // Assert
        Assert.False(_component.LoadingBDP);
        Assert.Equal(members, _component.NewMemberEntitiesBDP);
        Assert.Equal($"{members.Count} new member{(members.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_CallsExportService()
    {
        // Arrange
        var fileName = "test.xlsx";
        var members = new List<NewMemberEntity> { new() };
        _component.NewMemberEntitiesBDP = members;

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(service => service.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, members), Times.Once);
    }

    [Fact]
    public async Task LoadNewMembersAndManageUIAsync_ShowsAndHidesLoadingPanel()
    {
        // Arrange
        var members = new List<NewMemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveNewMembersDataAsync(_component.StartDate, _component.EndDate))
                            .ReturnsAsync(members);

        // Act
        await _component.LoadNewMembersAndManageUIAsync();

        // Assert
        _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.LoadingBDP);
    }
}