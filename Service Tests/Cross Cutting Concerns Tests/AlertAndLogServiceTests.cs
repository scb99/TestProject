using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class AlertAndLogServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _alertServiceMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerServiceMock;
    private readonly AlertAndLogService _alertAndLogService;

    public AlertAndLogServiceTests()
    {
        _alertServiceMock = new Mock<ICrossCuttingAlertService>();
        _loggerServiceMock = new Mock<ICrossCuttingLoggerService>();
        _alertAndLogService = new AlertAndLogService(_alertServiceMock.Object, _loggerServiceMock.Object);
    }

    [Fact]
    public async Task AlertAndLogErrorAsync_ShouldCallAlertAndLog_WhenCalledWithExceptionMessage()
    {
        // Arrange
        var fileName = "test.txt";
        var exceptionMessage = "An error occurred";

        // Act
        await _alertAndLogService.AlertAndLogErrorAsync(fileName, exceptionMessage);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Failed to export file {fileName}! Please check log for reason."), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync(exceptionMessage), Times.Once);
    }

    [Fact]
    public async Task AlertAndLogErrorAsync_ShouldCallAlertAndLog_WhenCalledWithGracePeriod()
    {
        // Arrange
        var fileName = "test.txt";
        var gracePeriod = 5;
        var exceptionMessage = "An error occurred";

        // Act
        await _alertAndLogService.AlertAndLogErrorAsync(fileName, gracePeriod, exceptionMessage);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Failed to export file {fileName} with grace period of {gracePeriod} days! Please check log for reason."), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync(exceptionMessage), Times.Once);
    }

    [Fact]
    public async Task AlertAndLogErrorAsync_ShouldCallAlertAndLog_WhenCalledWithDateRange()
    {
        // Arrange
        var fileName = "test.txt";
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var exceptionMessage = "An error occurred";

        // Act
        await _alertAndLogService.AlertAndLogErrorAsync(fileName, startDate, endDate, exceptionMessage);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Failed to export file {fileName} with start date {startDate.ToShortDateString()} and end date {endDate.ToShortDateString()}! Please check log for reason."), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync(exceptionMessage), Times.Once);
    }

    [Fact]
    public async Task AlertAndLogResultAsync_ShouldCallAlertAndLog_WhenCalledWithFileName()
    {
        // Arrange
        var fileName = "test.txt";

        // Act
        await _alertAndLogService.AlertAndLogResultAsync(fileName);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Please check your downloads folder for {fileName}!"), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync($"Successfully exported file {fileName}."), Times.Once);
    }

    [Fact]
    public async Task AlertAndLogResultAsync_ShouldCallAlertAndLog_WhenCalledWithGracePeriod()
    {
        // Arrange
        var fileName = "test.txt";
        var gracePeriod = 5;

        // Act
        await _alertAndLogService.AlertAndLogResultAsync(fileName, gracePeriod);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Please check your downloads folder for {fileName}!"), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync($"Successfully exported file {fileName} with grace period of {gracePeriod} days."), Times.Once);
    }

    [Fact]
    public async Task AlertAndLogResultAsync_ShouldCallAlertAndLog_WhenCalledWithDateRange()
    {
        // Arrange
        var fileName = "test.txt";
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        await _alertAndLogService.AlertAndLogResultAsync(fileName, startDate, endDate);

        // Assert
        _alertServiceMock.Verify(alert => alert.AlertUsingFallingMessageBoxAsync($"Please check your downloads folder for {fileName}!"), Times.Once);
        _loggerServiceMock.Verify(logger => logger.LogResultAsync($"Successfully exported file {fileName} with start date {startDate.ToShortDateString()} and end date {endDate.ToShortDateString()}."), Times.Once);
    }
}