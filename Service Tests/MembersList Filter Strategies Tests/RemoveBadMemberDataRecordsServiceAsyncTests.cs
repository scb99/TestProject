using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class RemoveBadMemberDataRecordsServiceAsyncTests
{
    [Fact]
    public async Task RemoveBadMemberDataRecordsServiceAsync_CallsDependenciesCorrectly()
    {
        // Arrange
        var mockCleanupService = new Mock<IMemberCleanupService>();
        var mockUserNotificationService = new Mock<IUserNotificationService>();
        int expectedCount = 5;

        mockCleanupService.Setup(service => service.RemoveBadMemberDataRecordsAsync())
            .ReturnsAsync(expectedCount);

        var service = new RemoveBadMemberDataRecordsService(mockCleanupService.Object, mockUserNotificationService.Object);

        // Act
        int result = await service.RemoveBadMemberDataRecordsServiceAsync();

        // Assert
        Assert.Equal(expectedCount, result);
        mockCleanupService.Verify(service => service.RemoveBadMemberDataRecordsAsync(), Times.Once);
        mockUserNotificationService.Verify(service => service.AlertAsync(It.IsAny<string>()), Times.Exactly(2));
        mockUserNotificationService.Verify(service => service.LogAsync(It.Is<string>(msg => msg.Contains($"{expectedCount} bad member data records"))), Times.Once);
    }
}