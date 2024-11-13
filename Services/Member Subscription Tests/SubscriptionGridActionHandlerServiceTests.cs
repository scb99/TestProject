using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MemberSubscription;

public class SubscriptionGridActionHandlerServiceTests
{
    private readonly Mock<ISubscriptionUpdateService> _updateServiceMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly SubscriptionGridActionHandlerService _service;

    public SubscriptionGridActionHandlerServiceTests()
    {
        _updateServiceMock = new Mock<ISubscriptionUpdateService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _service = new SubscriptionGridActionHandlerService(_updateServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleActionBeginAsync_BeginEdit_CallsCloneSubscriptionEntity()
    {
        // Arrange
        var subscriptionEntity = new SubscriptionEntity();
        var actionEventArgs = new ActionEventArgs<SubscriptionEntity> { RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit, Data = subscriptionEntity };

        // Act
        var result = await _service.HandleActionBeginAsync(actionEventArgs);

        // Assert
        _updateServiceMock.Verify(s => s.CloneSubscriptionEntity(subscriptionEntity), Times.Once);
        Assert.False(result);
    }

    [Fact]
    public async Task HandleActionBeginAsync_Save_CallsUpdateSubscriptionAsyncAndLogsMessage()
    {
        // Arrange
        var subscriptionEntity = new SubscriptionEntity { SubscriptionID = 1, Status = "Active", NextPaymentDateAsDateTime = DateTime.Now, UserID = 1 };
        var actionEventArgs = new ActionEventArgs<SubscriptionEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = subscriptionEntity };
        _updateServiceMock.Setup(s => s.UpdateSubscriptionAsync(actionEventArgs)).ReturnsAsync(true);

        // Act
        var result = await _service.HandleActionBeginAsync(actionEventArgs);

        // Assert
        _updateServiceMock.Verify(s => s.UpdateSubscriptionAsync(actionEventArgs), Times.Once);
        _loggerMock.Verify(s => s.LogResultAsync($"Updated subscription record with subscriptionID: {subscriptionEntity.SubscriptionID} status: {subscriptionEntity.Status} next payment date: {subscriptionEntity.NextPaymentDateAsDateTime} for {subscriptionEntity.UserID}"), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task HandleActionBeginAsync_Save_ReturnsFalseWhenUpdateFails()
    {
        // Arrange
        var subscriptionEntity = new SubscriptionEntity();
        var actionEventArgs = new ActionEventArgs<SubscriptionEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = subscriptionEntity };
        _updateServiceMock.Setup(s => s.UpdateSubscriptionAsync(actionEventArgs)).ReturnsAsync(false);

        // Act
        var result = await _service.HandleActionBeginAsync(actionEventArgs);

        // Assert
        _updateServiceMock.Verify(s => s.UpdateSubscriptionAsync(actionEventArgs), Times.Once);
        _loggerMock.Verify(s => s.LogResultAsync(It.IsAny<string>()), Times.Never);
        Assert.False(result);
    }
}