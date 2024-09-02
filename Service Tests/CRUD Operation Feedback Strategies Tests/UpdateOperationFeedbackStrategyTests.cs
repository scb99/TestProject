using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CRUDOperationFeedbackStrategy;

public class UpdateOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteSuccessAsync_CallsAlertServiceWithSuccessMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new UpdateOperationFeedbackStrategy(mockAlertService.Object);
        var successMessage = "Selected record successfully updated in DB!";

        // Act
        await strategy.ExecuteSuccessAsync();

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(successMessage), Times.Once);
    }

    [Fact]
    public async Task ExecuteFailureAsync_CallsAlertServiceWithFailureMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new UpdateOperationFeedbackStrategy(mockAlertService.Object);
        var failureMessage = "Selected record was NOT updated in DB!";

        // Act
        await strategy.ExecuteFailureAsync();

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}