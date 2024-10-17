using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberSubscription;

public class SubscriptionDataServiceTests
{
    private readonly Mock<IRepositoryPost> _postRepositoryMock;
    private readonly SubscriptionDataService _service;

    public SubscriptionDataServiceTests()
    {
        _postRepositoryMock = new Mock<IRepositoryPost>();
        _service = new SubscriptionDataService(_postRepositoryMock.Object);
    }

    [Fact]
    public async Task FetchSubscriptionDataAsync_CallsGetSubscriptionsByIDAsyncWithCorrectParameter()
    {
        // Arrange
        var selectedID = 1;
        var subscriptions = new List<SubscriptionEntity> { new() };
        _postRepositoryMock.Setup(repo => repo.GetSubscriptionsByIDAsync(selectedID)).ReturnsAsync(subscriptions);

        // Act
        var result = await _service.FetchSubscriptionDataAsync(selectedID);

        // Assert
        _postRepositoryMock.Verify(repo => repo.GetSubscriptionsByIDAsync(selectedID), Times.Once);
        Assert.Equal(subscriptions, result);
    }

    [Fact]
    public async Task FetchSubscriptionDataAsync_ReturnsCorrectResult()
    {
        // Arrange
        var selectedID = 1;
        var subscriptions = new List<SubscriptionEntity> { new() };
        _postRepositoryMock.Setup(repo => repo.GetSubscriptionsByIDAsync(selectedID)).ReturnsAsync(subscriptions);

        // Act
        var result = await _service.FetchSubscriptionDataAsync(selectedID);

        // Assert
        Assert.Equal(subscriptions, result);
    }
}