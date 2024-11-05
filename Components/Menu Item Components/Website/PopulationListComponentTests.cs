using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PopulationListComponentTests
{
    private readonly Mock<IRepository<PopulationEntity>> _mockPopulationRepository;
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryPopulation> _mockPopulationRepository2;
    private readonly PopulationListComponent _component;

    public PopulationListComponentTests()
    {
        _mockPopulationRepository = new Mock<IRepository<PopulationEntity>>();
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockPopulationRepository2 = new Mock<IRepositoryPopulation>();

        _component = new PopulationListComponent
        {
            PopulationRepository = _mockPopulationRepository.Object,
            DBOperationService = _mockDBOperationService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            LoggedInMemberService = _mockLoggedInMemberService.Object,
            Logger = _mockLogger.Object,
            Show = _mockShow.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            PopulationRepository2 = _mockPopulationRepository2.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldShowLoadingPanelAndLoadData()
    {
        // Arrange
        _mockPopulationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<PopulationEntity>());

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        _mockPopulationRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAndDisplayPopulationAndUpdateChartAsync_ShouldPopulatePopulationList()
    {
        // Arrange
        var populationEntities = new List<PopulationEntity>
        {
            new() { Year = 2020, Population = 1000 },
            new() { Year = 2021, Population = 2000 }
        };
        _mockPopulationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(populationEntities);

        // Act
        await _component.GetAndDisplayPopulationAndUpdateChartAsync();

        // Assert
        Assert.Equal(2, _component._populationList.Count);
        Assert.Equal("2020", _component._populationList[0].Year);
        Assert.Equal(1000, _component._populationList[0].Population);
        Assert.Equal("2021", _component._populationList[1].Year);
        Assert.Equal(2000, _component._populationList[1].Population);
    }

    [Fact]
    public async Task GetAndDisplayPopulationAndUpdateChartAsync_ShouldHandleNullPopulationEntities()
    {
        // Arrange
        var populationEntities = new List<PopulationEntity> { };
        _mockPopulationRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(populationEntities);

        // Act
        await _component.GetAndDisplayPopulationAndUpdateChartAsync();

        // Assert
        Assert.Empty(_component._populationList);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShouldHandleAddAction()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PopulationEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Add, Data = new PopulationEntity() };

        // Act
        await _component.OnActionBeginAsync(actionArgs);

        // Assert
        Assert.Equal(0, actionArgs.Data.ID);
        Assert.Equal(0, actionArgs.Data.Population);
        Assert.Equal(_mockSystemTimeService.Object.Now.Year, actionArgs.Data.Year);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShouldHandleDeleteAction()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PopulationEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Delete, Data = new PopulationEntity { ID = 1 } };
        _mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");
        _mockPopulationRepository.Setup(repo => repo.DeleteAsync(It.IsAny<PopulationEntity>())).ReturnsAsync(true);

        // Act
        await _component.OnActionBeginAsync(actionArgs);

        // Assert
        _mockPopulationRepository.Verify(repo => repo.DeleteAsync(It.IsAny<PopulationEntity>()), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShouldHandleSaveAction()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PopulationEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = new PopulationEntity { ID = 1, Year = 2022, Population = 3000 } };
        _component._clonedPopulationEntity = new PopulationEntity { ID = 1, Year = 2021, Population = 2000 };
        _mockPopulationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<PopulationEntity>())).ReturnsAsync(true);

        // Act
        await _component.OnActionBeginAsync(actionArgs);

        // Assert
        _mockPopulationRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PopulationEntity>()), Times.Once);
    }
}