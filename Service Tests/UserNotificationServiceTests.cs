using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class UserNotificationServiceTests
{
    [Fact]
    public async Task AlertAsync_CallsAlertServiceWithCorrectMessage()
    {
        // Arrange
        var mockAlertService = new Mock<IAlertService>();
        var mockLoggerService = new Mock<ILoggerService>();
        var userNotificationService = new UserNotificationService(mockAlertService.Object, mockLoggerService.Object);
        string testMessage = "Test Alert Message";

        // Act
        await userNotificationService.AlertAsync(testMessage);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(testMessage), Times.Once);
    }

    [Fact]
    public async Task LogAsync_CallsLoggerServiceWithCorrectMessage()
    {
        // Arrange
        var mockAlertService = new Mock<IAlertService>();
        var mockLoggerService = new Mock<ILoggerService>();
        var userNotificationService = new UserNotificationService(mockAlertService.Object, mockLoggerService.Object);
        string testMessage = "Test Log Message";

        // Act
        await userNotificationService.LogAsync(testMessage);

        // Assert
        mockLoggerService.Verify(service => service.LogResultAsync(testMessage), Times.Once);
    }
}