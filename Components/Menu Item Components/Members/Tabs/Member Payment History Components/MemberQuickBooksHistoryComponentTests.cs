using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace MenuItemComponents;

public class MemberQuickBooksHistoryComponentTests
{
    private readonly Mock<IRepositoryQuickBooks> _mockQuickBooksRepository;
    private readonly Mock<IRepositoryQuickBooks3> _mockQuickBooksRepository3;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly MemberQuickBooksHistoryComponent _component;

    public MemberQuickBooksHistoryComponentTests()
    {
        _mockQuickBooksRepository = new Mock<IRepositoryQuickBooks>();
        _mockQuickBooksRepository3 = new Mock<IRepositoryQuickBooks3>();
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();

        _component = new MemberQuickBooksHistoryComponent
        {
            QuickBooksRepository = _mockQuickBooksRepository.Object,
            QuickBooksRepository3 = _mockQuickBooksRepository3.Object,
            Show = _mockShow.Object,
            AllMembersInDBService = _mockAllMembersInDBService.Object,
            DBOperationService = _mockDBOperationService.Object,
            Logger = _mockLogger.Object,
            LoggedInMemberService = _mockLoggedInMemberService.Object,
            MemberNameService = _mockMemberNameService.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsQuickBooksDetails()
    {
        // Arrange
        var selectedID = 1;
        var quickBooksEntities = new List<QuickBooksEntity> { new() };
        _mockQuickBooksRepository.Setup(repo => repo.GetQuickBooksDetailsAsync(selectedID)).ReturnsAsync(quickBooksEntities);
        _mockMemberNameService.Setup(service => service.MemberName).Returns("Test Member");
        _component.Initialize(selectedID);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(quickBooksEntities, _component.QuickBooksEntitiesBDP);
        Assert.Equal("1 QuickBooks entry for Test Member", _component.TitleBDP);
        Assert.Equal("Test Member's QuickBooks records (which are not editable)", _component.TitleNoUpdateBDP);
    }

    [Fact]
    public async Task OnToolBarClickAsync_ShowsAlertWhenNoMemberSelected()
    {
        // Arrange
        var arg = new ClickEventArgs();
        _component.Initialize(0);

        // Act
        await _component.OnToolBarClickAsync(arg);

        // Assert
        Assert.True(arg.Cancel);
        _mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync("Please select a member first!"), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_DeletesRecordWhenSuperUser()
    {
        // Arrange
        var arg = new ActionEventArgs<QuickBooksEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Delete, Data = new QuickBooksEntity { ID = 1, MembershipID = 1, Payment = "100", UserID = 1 } };
        _mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");
        _mockQuickBooksRepository3.Setup(repo => repo.DeleteQuickBooksRecordAsync(arg.Data.ID)).ReturnsAsync(true);
        _mockAllMembersInDBService.Setup(service => service.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Test User" } });

        // Act
        await _component.OnActionBeginAsync(arg);

        // Assert
        _mockLogger.Verify(logger => logger.LogResultAsync(It.IsAny<string>()), Times.Once);
        _mockDBOperationService.Verify(service => service.AfterSuccessfulDBOperationAsync(DBOperation.Delete), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShowsNotAuthorizedWhenNotSuperUser()
    {
        // Arrange
        var arg = new ActionEventArgs<QuickBooksEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Delete, Data = new QuickBooksEntity { ID = 1 } };
        _mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("User");

        // Act
        await _component.OnActionBeginAsync(arg);

        // Assert
        Assert.True(arg.Cancel);
        _mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync("You are not authorized to do this operation!"), Times.Once);
    }
}