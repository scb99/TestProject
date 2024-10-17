using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberSubscription;

public class SubscriptionTitleGenerationServiceTests
{
    private readonly Mock<ICrossCuttingMemberNameService> _memberNameServiceMock;
    private readonly SubscriptionTitleGenerationService _service;

    public SubscriptionTitleGenerationServiceTests()
    {
        _memberNameServiceMock = new Mock<ICrossCuttingMemberNameService>();
        _service = new SubscriptionTitleGenerationService(_memberNameServiceMock.Object);
    }

    [Fact]
    public void GenerateSubscriptionTitle_SingleSubscription_ReturnsCorrectTitle()
    {
        // Arrange
        var subscriptionEntities = new List<SubscriptionEntity> { new() };
        _memberNameServiceMock.Setup(s => s.MemberName).Returns("John Doe");

        // Act
        var result = _service.GenerateSubscriptionTitle(subscriptionEntities);

        // Assert
        Assert.Equal("1 Subscription entry for John Doe", result);
    }

    [Fact]
    public void GenerateSubscriptionTitle_MultipleSubscriptions_ReturnsCorrectTitle()
    {
        // Arrange
        var subscriptionEntities = new List<SubscriptionEntity> { new(), new() };
        _memberNameServiceMock.Setup(s => s.MemberName).Returns("John Doe");

        // Act
        var result = _service.GenerateSubscriptionTitle(subscriptionEntities);

        // Assert
        Assert.Equal("2 Subscription entries for John Doe", result);
    }

    [Fact]
    public void GenerateSubscriptionTitle_EmptyList_ReturnsCorrectTitle()
    {
        // Arrange
        var subscriptionEntities = new List<SubscriptionEntity>();
        _memberNameServiceMock.Setup(s => s.MemberName).Returns("John Doe");

        // Act
        var result = _service.GenerateSubscriptionTitle(subscriptionEntities);

        // Assert
        Assert.Equal("0 Subscription entries for John Doe", result);
    }
}