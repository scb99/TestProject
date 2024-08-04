using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class FindNewMembersComponentTests
{
    private readonly Mock<IFindNewMembersDataService> _mockDataService;
    private readonly Mock<IFindNewMembersExportService> _mockExportService;
    private readonly Mock<ICrossCuttingLoadingService> _mockLoadingService;
    private readonly FindNewMembersComponent _component;

    public FindNewMembersComponentTests()
    {
        _mockDataService = new Mock<IFindNewMembersDataService>();
        _mockExportService = new Mock<IFindNewMembersExportService>();
        _mockLoadingService = new Mock<ICrossCuttingLoadingService>();

        _component = new FindNewMembersComponent
        {
            FindNewMembersDataService = _mockDataService.Object,
            FindNewMembersExportService = _mockExportService.Object,
            LoadingService = _mockLoadingService.Object,
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldFetchNewMembers()
    {
        // Arrange
        var newMembers = new List<NewMemberEntity>
        {
            new() { ID = 1, Name = "John Doe" },
            new() { ID = 2, Name = "Jane Smith" }
        };
        _mockDataService.Setup(s => s.FetchNewMembersDataAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                        .ReturnsAsync(newMembers);
        _mockLoadingService.Setup(s => s.ShowSpinnersExecuteHideSpinnersAsync(It.IsAny<Func<Task>>(), It.IsAny<Action<bool>>()))
                           .Callback<Func<Task>, Action<bool>>((action, onLoadingStateChanged) => action.Invoke());

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(newMembers, _component.NewMemberEntitiesBDP);
        Assert.Equal($"{newMembers.Count} new member{(newMembers.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallExportService()
    {
        // Arrange
        var fileName = "test.xlsx";
        _component.NewMemberEntitiesBDP = new List<NewMemberEntity>
        {
            new() { ID = 1, Name = "John Doe" }
        };

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(s => s.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, _component.NewMemberEntitiesBDP), Times.Once);
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldFetchNewMembersData2()
    {
        // Arrange
        var newMembers = new List<NewMemberEntity> { new() };

        _mockDataService.Setup(ds => ds.FetchNewMembersDataAsync(_component.StartDate, _component.EndDate))
            .ReturnsAsync(newMembers);

        _mockLoadingService.Setup(ls => ls.ShowSpinnersExecuteHideSpinnersAsync(It.IsAny<Func<Task>>(), It.IsAny<Action<bool>>()))
            .Callback<Func<Task>, Action<bool>>(async (func, setLoading) =>
            {
                setLoading(true);
                await func();
                setLoading(false);
            });

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(newMembers, _component.NewMemberEntitiesBDP);
        Assert.Equal($"{newMembers.Count} new member{(newMembers.Count == 1 ? "" : "s")} joined STPC between {_component.StartDate.ToShortDateString()} and {_component.EndDate.ToShortDateString()}", _component.TitleBDP);
        Assert.False(_component.LoadingBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallExportService2()
    {
        // Arrange
        var fileName = "test.xlsx";
        var now = DateTime.Now;
        var newMembers = new List<NewMemberEntity> { new() };

        _component.NewMemberEntitiesBDP = newMembers;
        _component.Now = now;

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(es => es.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, now, _component.ExcelGrid, newMembers), Times.Once);
    }
}