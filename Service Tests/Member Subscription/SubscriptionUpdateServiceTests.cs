using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MemberSubscription;

public class SubscriptionUpdateServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _alertServiceMock;
    private readonly Mock<IRepositoryPosts2> _postsRepository2Mock;
    private readonly Mock<IRepositoryPostmeta3> _postmetaRepository3Mock;
    private readonly SubscriptionUpdateService _service;

    public SubscriptionUpdateServiceTests()
    {
        _alertServiceMock = new Mock<ICrossCuttingAlertService>();
        _postsRepository2Mock = new Mock<IRepositoryPosts2>();
        _postmetaRepository3Mock = new Mock<IRepositoryPostmeta3>();
        _service = new SubscriptionUpdateService(_alertServiceMock.Object, _postsRepository2Mock.Object, _postmetaRepository3Mock.Object);
    }

    [Fact]
    public void CloneSubscriptionEntity_ClonesEntityCorrectly()
    {
        // Arrange
        var entity = new SubscriptionEntity { SubscriptionID = 1, Status = "Active", NextPaymentDateAsDateTime = DateTime.Now };

        // Act
        _service.CloneSubscriptionEntity(entity);

        // Assert
        var clonedEntityField = _service.GetType().GetField("_clonedSubscriptionEntity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var clonedEntity = clonedEntityField?.GetValue(_service) as SubscriptionEntity;
        Assert.NotNull(clonedEntity);
        Assert.Equal(entity.SubscriptionID, clonedEntity.SubscriptionID);
        Assert.Equal(entity.Status, clonedEntity.Status);
        Assert.Equal(entity.NextPaymentDateAsDateTime, clonedEntity.NextPaymentDateAsDateTime);
    }

    [Fact]
    public async Task UpdateSubscriptionAsync_StatusDirty_CallsUpdateSubscriptionStatusAsync()
    {
        // Arrange
        var originalEntity = new SubscriptionEntity { SubscriptionID = 1, Status = "Active" };
        var updatedEntity = new SubscriptionEntity { SubscriptionID = 1, Status = "Inactive" };
        _service.CloneSubscriptionEntity(originalEntity);
        var actionEventArgs = new ActionEventArgs<SubscriptionEntity> { Data = updatedEntity };
        _postsRepository2Mock.Setup(repo => repo.UpdateSubscriptionStatusAsync(updatedEntity.SubscriptionID, "wc-Inactive")).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateSubscriptionAsync(actionEventArgs);

        // Assert
        _postsRepository2Mock.Verify(repo => repo.UpdateSubscriptionStatusAsync(updatedEntity.SubscriptionID, "wc-Inactive"), Times.Once);
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Subscription status updated!"), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSubscriptionAsync_NextPaymentDateDirty_CallsUpdateSubscriptionNextPaymentDateAsync()
    {
        // Arrange
        var originalEntity = new SubscriptionEntity { SubscriptionID = 1, NextPaymentDateAsDateTime = DateTime.Now.AddDays(-1), NextPaymentDate = "1" };
        var updatedEntity = new SubscriptionEntity { SubscriptionID = 1, NextPaymentDateAsDateTime = DateTime.Now, NextPaymentDate = "0" };
        _service.CloneSubscriptionEntity(originalEntity);
        var actionEventArgs = new ActionEventArgs<SubscriptionEntity> { Data = updatedEntity };
        _postmetaRepository3Mock.Setup(repo => repo.UpdateSubscriptionNextPaymentDateAsync(updatedEntity.SubscriptionID, updatedEntity.NextPaymentDateAsDateTime.ConvertToStringDate())).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateSubscriptionAsync(actionEventArgs);

        // Assert
        _postmetaRepository3Mock.Verify(repo => repo.UpdateSubscriptionNextPaymentDateAsync(updatedEntity.SubscriptionID, updatedEntity.NextPaymentDateAsDateTime.ConvertToStringDate()), Times.Once);
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Subscription next payment date updated!"), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSubscriptionStatusAsync_UpdatesStatusAndAlertsCorrectly()
    {
        // Arrange
        var entity = new SubscriptionEntity { SubscriptionID = 1, Status = "Inactive" };
        _service.CloneSubscriptionEntity(entity);
        _postsRepository2Mock.Setup(repo => repo.UpdateSubscriptionStatusAsync(entity.SubscriptionID, "wc-Inactive")).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateSubscriptionStatusAsync(entity);

        // Assert
        _postsRepository2Mock.Verify(repo => repo.UpdateSubscriptionStatusAsync(entity.SubscriptionID, "wc-Inactive"), Times.Once);
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Subscription status updated!"), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateSubscriptionNextPaymentDateAsync_UpdatesNextPaymentDateAndAlertsCorrectly()
    {
        // Arrange
        var entity = new SubscriptionEntity { SubscriptionID = 1, NextPaymentDateAsDateTime = DateTime.Now };
        _service.CloneSubscriptionEntity(entity);
        var nextPaymentDateString = entity.NextPaymentDateAsDateTime.ConvertToStringDate();
        _postmetaRepository3Mock.Setup(repo => repo.UpdateSubscriptionNextPaymentDateAsync(entity.SubscriptionID, nextPaymentDateString)).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateSubscriptionNextPaymentDateAsync(entity);

        // Assert
        _postmetaRepository3Mock.Verify(repo => repo.UpdateSubscriptionNextPaymentDateAsync(entity.SubscriptionID, nextPaymentDateString), Times.Once);
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync("Subscription next payment date updated!"), Times.Once);
        Assert.True(result);
    }
}