using DataAccess;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class LoggerServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService = new();
    private readonly Mock<ILoggedInMemberService> _mockLoggedInMemberService = new();
    private readonly Mock<ILogToDB> _mockLogToDB = new();
    private readonly LoggerService _loggerService;

    public LoggerServiceTests()
    {
        _loggerService = new LoggerService(_mockAlertService.Object, _mockLoggedInMemberService.Object, _mockLogToDB.Object);
        _mockLoggedInMemberService.Setup(s => s.MemberUserID).Returns(1); // Assuming MemberUserID is an int
    }

    [Fact]
    public async Task LogExceptionAsync_WithContext_LogsAndAlerts()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var context = "TestContext";

        // Act
        await _loggerService.LogExceptionAsync(exception, context);

        // Assert
        _mockLogToDB.Verify(log => log.LogErrorToDBAsync(It.IsAny<int>(), It.Is<string>(s => s.Contains("Test exception") && s.Contains("TestContext"))), Times.Once);
        _mockAlertService.Verify(alert => alert.AlertUsingPopUpMessageBoxAsync(It.Is<string>(s => s.Contains("TestContext"))), Times.Once);
    }

    [Fact]
    public async Task LogExceptionAsync_WithMethodNameAndClassName_LogsAndAlerts()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var methodName = "TestMethod";
        var className = "TestClass";

        // Act
        await _loggerService.LogExceptionAsync(exception, methodName, className);

        // Assert
        _mockLogToDB.Verify(log => log.LogErrorToDBAsync(It.IsAny<int>(), It.Is<string>(s => s.Contains("Test exception") && s.Contains("TestMethod") && s.Contains("TestClass"))), Times.Once);
        _mockAlertService.Verify(alert => alert.AlertUsingFallingMessageBoxAsync(It.Is<string>(s => s.Contains("TestMethod") && s.Contains("TestClass"))), Times.Once);
    }

    [Fact]
    public async Task LogResultAsync_LogsMessage()
    {
        // Arrange
        var message = "Test result message";

        // Act
        await _loggerService.LogResultAsync(message);

        // Assert
        _mockLogToDB.Verify(log => log.LogInfoToDBAsync(It.IsAny<int>(), It.Is<string>(s => s == message)), Times.Once);
    }
}