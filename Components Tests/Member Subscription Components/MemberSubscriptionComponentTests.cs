using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace MenuItemComponents;

public class MemberSubscriptionComponentTests
{
    private readonly MemberSubscriptionComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<IDataManager> _mockDataManager;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;

    public MemberSubscriptionComponentTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockAllMembersInDBService.Setup(am => am.MemberNameDictionary).Returns(new Dictionary<int, string> { { 0, "Test Member" } });
        _mockDataManager = new Mock<IDataManager>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();

        _component = new MemberSubscriptionComponent
        {
            Show = _mockAlertService.Object,
            AllMembersInDBService = _mockAllMembersInDBService.Object,
            DataManager = _mockDataManager.Object,
            Logger = _mockLogger.Object,
            MemberNameService = _mockMemberNameService.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldFetchDataAndGenerateTitle_WhenSelectedIDIsNotZero()
    {
        // Arrange
        _component.SelectedID = 1;
        var subscriptions = new List<SubscriptionEntity> { new() };
        _mockDataManager.Setup(dm => dm.GetSubscriptionsByIDSPAsync(It.IsAny<int>())).ReturnsAsync(subscriptions);
        _mockMemberNameService.Setup(mns => mns.MemberName).Returns("Test Member");

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(subscriptions, _component.SubscriptionEntitiesBDP);
        Assert.Equal("1 Subscription entry for Test Member", _component.TitleBDP);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShouldCloneData_OnBeginEdit()
    {
        // Arrange
        var subscription = new SubscriptionEntity { SubscriptionID = 1, Status = "Active" };
        var args = new ActionEventArgs<SubscriptionEntity> { RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit, Data = subscription };

        // Act
        await _component.OnActionBeginAsync(args);

        // Assert
        Assert.Equal(subscription.SubscriptionID, _component.clonedSubscriptionEntity.SubscriptionID);
        Assert.Equal(subscription.Status, _component.clonedSubscriptionEntity.Status);
    }

    [Fact]
    public async Task OnActionBeginAsync_ShouldUpdateData_OnSave()
    {
        // Arrange
        var subscription = new SubscriptionEntity { SubscriptionID = 1, Status = "Active" };
        var args = new ActionEventArgs<SubscriptionEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = subscription };
        _component.clonedSubscriptionEntity = new SubscriptionEntity { SubscriptionID = 1, Status = "Inactive" };
        _mockDataManager.Setup(dm => dm.UpdateSubscriptionStatusSPAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

        // Act
        await _component.OnActionBeginAsync(args);

        // Assert
        _mockDataManager.Verify(dm => dm.UpdateSubscriptionStatusSPAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        _mockLogger.Verify(logger => logger.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnToolBarClickAsync_ShouldShowAlert_WhenSelectedIDIsZero()
    {
        // Arrange
        _component.SelectedID = 0;
        var args = new ClickEventArgs();

        // Act
        await _component.OnToolBarClickAsync(args);

        // Assert
        Assert.True(args.Cancel);
        _mockAlertService.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Please select a member first!"), Times.Once);
    }
}