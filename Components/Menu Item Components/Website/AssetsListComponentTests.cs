using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class AssetsListComponentTests
{
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepository<AssetsEntity>> _mockAssetRepository;
    private readonly Mock<IRepositoryAsset> _mockAssetRepository2;
    private readonly AssetsListComponent _component;

    public AssetsListComponentTests()
    {
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockAssetRepository = new Mock<IRepository<AssetsEntity>>();
        _mockAssetRepository2 = new Mock<IRepositoryAsset>();

        _component = new AssetsListComponent
        {
            DBOpertionService = _mockDBOperationService.Object,
            LoggedInMemberService = _mockLoggedInMemberService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLoggerService.Object,
            Show = _mockAlertService.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            AssetRepository = _mockAssetRepository.Object,
            AssetRepository2 = _mockAssetRepository2.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsLoadingBDPToTrueAndCallsGetAndDisplayAssetsAndUpdateChartAsync()
    {
        // Arrange
        _mockLoadingPanelService.Setup(x => x.ShowLoadingPanelAsync()).Returns(Task.CompletedTask);
        var mockAssets = new List<AssetsEntity>
        {
            new() { Year = 2020, Amount = "1000" },
            new() { Year = 2021, Amount = "2000" }
        };
        _mockAssetRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(mockAssets);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.False(_component.LoadingBDP);
        _mockLoadingPanelService.Verify(x => x.ShowLoadingPanelAsync(), Times.Once);
        _mockLoggerService.Verify(x => x.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAndDisplayAssetsAndUpdateChartAsync_PopulatesAssetsEntitiesToDisplayBDPAndAssetsList()
    {
        // Arrange
        var mockAssets = new List<AssetsEntity>
        {
            new() { Year = 2020, Amount = "1000" },
            new() { Year = 2021, Amount = "2000" }
        };
        _mockAssetRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(mockAssets);

        // Act
        await _component.GetAndDisplayAssetsAndUpdateChartAsync();

        // Assert
        Assert.Equal(2, _component.AssetsEntitiesToDisplayBDP.Count);
        Assert.Equal(2, _component._assetsList.Count);
        Assert.Equal("2020", _component._assetsList[0].Year);
        Assert.Equal(1000, _component._assetsList[0].AssetsValue);
    }

    //[Fact]
    //public async Task OnActionBeginAsync_CallsHandleActionAsync()
    //{
    //    // Arrange
    //    var actionArgs = new ActionEventArgs<AssetsEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Add, Data = new AssetsEntity() };
    //    var actionHandler = new Mock<AssetsActionHandler>(_component);
    //    actionHandler.Setup(x => x.HandleActionAsync(actionArgs)).Returns(Task.CompletedTask);

    //    // Act
    //    await _component.OnActionBeginAsync(actionArgs);

    //    // Assert
    //    actionHandler.Verify(x => x.HandleActionAsync(actionArgs), Times.Once);
    //}

    [Fact]
    public async Task DisplayAlertMessageAsync_CallsAlertUsingPopUpMessageBoxAsync()
    {
        // Arrange
        var message = "Test message";

        // Act
        var result = await _component.DisplayAlertMessageAsync(message);

        // Assert
        Assert.Equal(0, result);
        _mockAlertService.Verify(x => x.AlertUsingPopUpMessageBoxAsync(message), Times.Once);
    }
}