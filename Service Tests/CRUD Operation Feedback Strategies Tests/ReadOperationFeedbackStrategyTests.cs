using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CRUDOperationFeedbackStrategy;

public class ReadOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteFailureAsync_CallsAlertServiceWithCorrectErrorMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new ReadOperationFeedbackStrategy();
        var errorMessage = "Error accessing database";
        var expectedMessage = $"Tried to read record and got message:{errorMessage}";

        // Act
        await strategy.ExecuteFailureAsync(mockAlertService.Object, errorMessage);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(expectedMessage), Times.Once);
    }

    // Since ExecuteSuccessAsync is not implemented, it's not tested here.
}