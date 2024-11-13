using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Services;

public class RemoveBadMemberDataRecordsServiceTests
{
    private readonly Mock<IMemberCleanupService> _mockCleanupService;
    private readonly Mock<IUserNotificationService> _mockUserNotificationService;
    private readonly RemoveBadMemberDataRecordsService _service;

    public RemoveBadMemberDataRecordsServiceTests()
    {
        _mockCleanupService = new Mock<IMemberCleanupService>();
        _mockUserNotificationService = new Mock<IUserNotificationService>();

        _service = new RemoveBadMemberDataRecordsService(
            _mockCleanupService.Object,
            _mockUserNotificationService.Object
        );
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsServiceAsync_AlertsAndLogsCorrectly()
    {
        // Arrange
        var expectedCount = 5;
        _mockCleanupService.Setup(service => service.RemoveBadMemberDataRecordsAsync()).ReturnsAsync(expectedCount);

        // Act
        var result = await _service.RemoveBadMemberDataRecordsServiceAsync();

        // Assert
        Assert.Equal(expectedCount, result);
        _mockUserNotificationService.Verify(service => service.AlertAsync("Removing bad member data records (this may take some time)..."), Times.Once);
        _mockUserNotificationService.Verify(service => service.LogAsync($"Removed {expectedCount} bad member data records."), Times.Once);
        _mockUserNotificationService.Verify(service => service.AlertAsync($"{expectedCount} bad member data records removed."), Times.Once);
    }
}