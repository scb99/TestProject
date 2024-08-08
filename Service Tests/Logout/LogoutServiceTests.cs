using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Logout;

public class LogoutServiceTests
{
    [Fact]
    public async Task HandleLogoutAsync_CanLogout_PerformsLogoutAndNavigatesToRoot()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockLogoutBusinessLogicService = new Mock<ILogoutBusinessLogicService>();
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();

        mockLogoutBusinessLogicService.Setup(service => service.CanLogoutAsync()).ReturnsAsync(true);
        var logoutService = new LogoutService(
            mockAlertService.Object,
            mockLogger.Object,
            mockLogoutBusinessLogicService.Object,
            mockNavigationService.Object
        );

        // Act
        await logoutService.HandleLogoutAsync();

        // Assert
        mockLogoutBusinessLogicService.Verify(service => service.PerformLogoutAsync(), Times.Once);
        mockNavigationService.Verify(service => service.NavigateTo("/"), Times.Once);
    }

    [Fact]
    public async Task HandleLogoutAsync_CannotLogout_ShowsAlertAndNavigatesToMembersPage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockLogoutBusinessLogicService = new Mock<ILogoutBusinessLogicService>();
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();

        mockLogoutBusinessLogicService.Setup(service => service.CanLogoutAsync()).ReturnsAsync(false);
        var logoutService = new LogoutService(
            mockAlertService.Object,
            mockLogger.Object,
            mockLogoutBusinessLogicService.Object,
            mockNavigationService.Object
        );

        // Act
        await logoutService.HandleLogoutAsync();

        // Assert
        mockAlertService.Verify(service => service.AlertUsingPopUpMessageBoxAsync(It.IsAny<string>()), Times.Once);
        mockNavigationService.Verify(service => service.NavigateTo("MembersPage"), Times.Once);
    }

    [Fact]
    public async Task HandleLogoutAsync_ExceptionThrown_LogsException()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockLogoutBusinessLogicService = new Mock<ILogoutBusinessLogicService>();
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();

        mockLogoutBusinessLogicService.Setup(service => service.CanLogoutAsync()).ReturnsAsync(true);
        mockLogoutBusinessLogicService.Setup(service => service.PerformLogoutAsync()).ThrowsAsync(new Exception("Test Exception"));
        var logoutService = new LogoutService(
            mockAlertService.Object,
            mockLogger.Object,
            mockLogoutBusinessLogicService.Object,
            mockNavigationService.Object
        );

        // Act
        await logoutService.HandleLogoutAsync();

        // Assert
        mockLogger.Verify(service => service.LogExceptionAsync(It.IsAny<Exception>(), nameof(logoutService.HandleLogoutAsync)), Times.Once);
    }

    [Fact]
    public async Task HandleCancelLogoutAsync_NavigatesToWelcomePage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockLogoutBusinessLogicService = new Mock<ILogoutBusinessLogicService>();
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();

        var logoutService = new LogoutService(
            mockAlertService.Object,
            mockLogger.Object,
            mockLogoutBusinessLogicService.Object,
            mockNavigationService.Object
        );

        // Act
        await logoutService.HandleCancelLogoutAsync();

        // Assert
        mockNavigationService.Verify(service => service.NavigateTo("WelcomePage"), Times.Once);
    }

    [Fact]
    public async Task HandleCancelLogoutAsync_ExceptionThrown_LogsException()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockLogoutBusinessLogicService = new Mock<ILogoutBusinessLogicService>();
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();

        mockNavigationService.Setup(service => service.NavigateTo("WelcomePage")).Throws(new Exception("Test Exception"));
        var logoutService = new LogoutService(
            mockAlertService.Object,
            mockLogger.Object,
            mockLogoutBusinessLogicService.Object,
            mockNavigationService.Object
        );

        // Act
        await logoutService.HandleCancelLogoutAsync();

        // Assert
        mockLogger.Verify(service => service.LogExceptionAsync(It.IsAny<Exception>(), nameof(logoutService.HandleCancelLogoutAsync)), Times.Once);
    }
}