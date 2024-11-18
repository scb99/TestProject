using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class FindNewMembersComponentTests : FindNewMembersComponent
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

        _component = new FindNewMembersComponent();

        _component.SetPrivatePropertyValue("LoadingPanelService", _mockLoadingPanelService.Object);
        _component.SetPrivatePropertyValue("FindNewMembersExportService", _mockExportService.Object);
        _component.SetPrivatePropertyValue("RetrieveNewMembersDataService", _mockRetrieveService.Object);

        _component.SetPublicPropertyValue(nameof(StartDate), DateTime.Now.AddDays(-30));
        _component.SetPublicPropertyValue(nameof(EndDate), DateTime.Now);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsNewMemberEntitiesAndTitle()
    {
        // Arrange
        var members = new List<NewMemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveNewMembersDataAsync(_component.StartDate, _component.EndDate))
                            .ReturnsAsync(members);

        // Act
        await typeof(FindNewMembersComponent).InvokeAsync(nameof(OnParametersSetAsync), _component);

        // Assert
        Assert.Equal(members, _component.GetPrivatePropertyValue<List<NewMemberEntity>>("NewMemberEntitiesBDP"));
        Assert.Equal($"{members.Count} new member{(members.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.GetPrivatePropertyValue<string>("TitleBDP"));
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
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
        Assert.Equal(members, _component.GetPrivatePropertyValue<List<NewMemberEntity>>("NewMemberEntitiesBDP"));
        Assert.Equal($"{members.Count} new member{(members.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.GetPrivatePropertyValue<string>("TitleBDP"));
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_CallsExportService()
    {
        // Arrange
        var fileName = "test.xlsx";
        var members = new List<NewMemberEntity> { new() };
        _component.SetPrivatePropertyValue("NewMemberEntitiesBDP", members);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(service => service.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate,
            _component.GetPrivatePropertyValue<DateTime>("Now"),
            _component.GetPrivatePropertyValue<SfGrid<NewMemberEntity>>("ExcelGrid"),
            members), Times.Once);
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
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
    }
}