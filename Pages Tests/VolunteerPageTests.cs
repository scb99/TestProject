using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class VolunteersPageTests
{
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IRetrieveVolunteersDataService> _mockRetrieveVolunteersDataService;
    private readonly Mock<IStateManagementVolunteersPageService> _mockStateManagement;
    private readonly Mock<IUIEventHandlerVolunteersPageService> _mockUIEventHandlerService;
    private readonly VolunteersPage _volunteersPage;

    public VolunteersPageTests()
    {
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockRetrieveVolunteersDataService = new Mock<IRetrieveVolunteersDataService>();
        _mockStateManagement = new Mock<IStateManagementVolunteersPageService>();
        _mockUIEventHandlerService = new Mock<IUIEventHandlerVolunteersPageService>();

        _volunteersPage = new VolunteersPage
        {
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            RetrieveVolunteersDataService = _mockRetrieveVolunteersDataService.Object,
            StateManagement = _mockStateManagement.Object,
            UIEventHandlerService = _mockUIEventHandlerService.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_ShouldCallLoadVolunteersAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadVolunteersAndManageUIAsyncCalled = false;
    //    var volunteerLists = new VolunteerLists();
    //    var volunteerEntitiesBDP = new List<Volunteer>();
    //    var volunteersEntities2BDP = new List<TypeOfVolunteer>();
    //    _mockRetrieveVolunteersDataService.Setup(s => s.RetrieveVolunteersDataAsync())
    //        .ReturnsAsync(new VolunteerLists());
    //    _mockStateManagement.Setup(s => s.ManageState(It.IsAny<VolunteerLists>()))
    //        .Returns(new Tuple<VolunteerLists, List<Volunteer>, List<TypeOfVolunteer>>(volunteerLists, volunteerEntitiesBDP, volunteersEntities2BDP));
    //    _volunteersPage.GetType().GetMethod("LoadVolunteersAndManageUIAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    //        ?.Invoke(_volunteersPage, new object[] { });

    //    // Act
    //    await _volunteersPage.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadVolunteersAndManageUIAsyncCalled);
    //}

    [Fact]
    public async Task LoadVolunteersAndManageUIAsync_ShouldSetLoadingBDPToFalse()
    {
        // Arrange
        var volunteerLists = new VolunteerLists();
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        _mockRetrieveVolunteersDataService.Setup(s => s.RetrieveVolunteersDataAsync())
            .ReturnsAsync(new VolunteerLists());
        _mockStateManagement.Setup(s => s.ManageState(It.IsAny<VolunteerLists>()))
            .Returns(new Tuple<VolunteerLists, List<Volunteer>, List<TypeOfVolunteer>>(volunteerLists, volunteerEntitiesBDP, volunteersEntities2BDP));

        // Act
        await _volunteersPage.LoadVolunteersAndManageUIAsync();

        // Assert
        _mockLoadingPanelService.Verify(s => s.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_volunteersPage.LoadingBDP);
    }

    [Fact]
    public async Task LoadVolunteersAndManageUIAsync_ShouldLogResultOnSuccess()
    {
        // Arrange
        var volunteerLists = new VolunteerLists();
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        _mockRetrieveVolunteersDataService.Setup(s => s.RetrieveVolunteersDataAsync())
            .ReturnsAsync(new VolunteerLists());
        _mockStateManagement.Setup(s => s.ManageState(It.IsAny<VolunteerLists>()))
            .Returns(new Tuple<VolunteerLists, List<Volunteer>, List<TypeOfVolunteer>>(volunteerLists, volunteerEntitiesBDP, volunteersEntities2BDP));

        // Act
        await _volunteersPage.LoadVolunteersAndManageUIAsync();

        // Assert
        _mockLogger.Verify(l => l.LogResultAsync("Displayed Volunteers List"), Times.Once);
    }

    [Fact]
    public async Task LoadVolunteersAndManageUIAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockRetrieveVolunteersDataService.Setup(s => s.RetrieveVolunteersDataAsync())
            .ThrowsAsync(exception);

        // Act
        await _volunteersPage.LoadVolunteersAndManageUIAsync();

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, "VolunteersPage.LoadVolunteersAndManageUIAsync"), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallHandleExportAsync()
    {
        // Arrange
        var fileName = "test.xlsx";
        _volunteersPage.volunteerLists = new VolunteerLists();
        _volunteersPage.VolunteersEntities2BDP = new List<TypeOfVolunteer>();
        _volunteersPage.ExcelGrid = new SfGrid<TypeOfVolunteer>();

        // Act
        await _volunteersPage.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockUIEventHandlerService.Verify(s => s.HandleExportAsync(fileName, _volunteersPage.volunteerLists, _volunteersPage.VolunteersEntities2BDP, _volunteersPage.ExcelGrid), Times.Once);
    }
}
//using DBExplorerBlazor.Interfaces;
//using DBExplorerBlazor.Pages;
//using Moq;
//using Syncfusion.Blazor.Grids;

//namespace MenuItemComponents;

//public class VolunteerPageTests
//{
//    private readonly Mock<ICrossCuttingLoadingService> _mockLoadingService;
//    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
//    private readonly Mock<IRetrieveVolunteersDataService> _mockRetrieveVolunteersDataService;
//    private readonly Mock<IStateManagementVolunteersPageService> _mockStateManagement;
//    private readonly Mock<IUIEventHandlerVolunteersPageService> _mockUIEventHandlerService;
//    private readonly VolunteersPage _volunteersPage;

//    public VolunteerPageTests()
//    {
//        _mockLoadingService = new Mock<ICrossCuttingLoadingService>();
//        _mockLogger = new Mock<ICrossCuttingLoggerService>();
//        _mockRetrieveVolunteersDataService = new Mock<IRetrieveVolunteersDataService>();
//        _mockStateManagement = new Mock<IStateManagementVolunteersPageService>();
//        _mockUIEventHandlerService = new Mock<IUIEventHandlerVolunteersPageService>();

//        _volunteersPage = new VolunteersPage
//        {
//            LoadingService = _mockLoadingService.Object,
//            Logger = _mockLogger.Object,
//            RetrieveVolunteersDataService = _mockRetrieveVolunteersDataService.Object,
//            StateManagement = _mockStateManagement.Object,
//            UIEventHandlerService = _mockUIEventHandlerService.Object
//        };
//    }

//    [Fact]
//    public async Task OnParametersSetAsync_ShouldLoadDataAndLogResult()
//    {
//        // Arrange
//        var volunteerLists = new VolunteerLists();
//        var volunteerEntitiesBDP = new List<Volunteer>();
//        var volunteersEntities2BDP = new List<TypeOfVolunteer>();

//        _mockRetrieveVolunteersDataService
//            .Setup(service => service.RetrieveVolunteersDataAsync())
//            .ReturnsAsync(volunteerLists);

//        _mockStateManagement
//            .Setup(service => service.ManageState(volunteerLists))
//            .Returns(new Tuple<VolunteerLists, List<Volunteer>, List<TypeOfVolunteer>>(volunteerLists, volunteerEntitiesBDP, volunteersEntities2BDP));

//        _mockLoadingService
//            .Setup(service => service.ShowSpinnersExecuteHideSpinnersAsync(It.IsAny<Func<Task>>(), It.IsAny<Action<bool>>()))
//            .Callback<Func<Task>, Action<bool>>(async (func, setLoading) =>
//            {
//                setLoading(true);
//                await func();
//                setLoading(false);
//            });

//        // Act
//        await _volunteersPage.OnParametersSet2Async();

//        // Assert
//        Assert.Equal(volunteerLists, _volunteersPage.volunteerLists);
//        Assert.Equal(volunteerEntitiesBDP, _volunteersPage.VolunteerEntitiesBDP);
//        Assert.Equal(volunteersEntities2BDP, _volunteersPage.VolunteersEntities2BDP);
//        _mockLogger.Verify(logger => logger.LogResultAsync("Displayed Volunteers List"), Times.Once);
//    }

//    [Fact]
//    public async Task OnClickExportSpreadsheetDataAsync_ShouldCallHandleExportAsync()
//    {
//        // Arrange
//        var fileName = "test.xlsx";
//        var volunteerLists = new VolunteerLists();
//        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
//        var excelGrid = new Mock<SfGrid<TypeOfVolunteer>>();

//        _volunteersPage.volunteerLists = volunteerLists;
//        _volunteersPage.VolunteersEntities2BDP = volunteersEntities2BDP;
//        _volunteersPage.ExcelGrid = excelGrid.Object;

//        // Act
//        await _volunteersPage.OnClickExportSpreadsheetDataAsync(fileName);

//        // Assert
//        _mockUIEventHandlerService.Verify(service => service.HandleExportAsync(fileName, volunteerLists, volunteersEntities2BDP, excelGrid.Object), Times.Once);
//    }
//}