using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CRUDOperationFeedbackStrategy;

public class CreateOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteSuccessAsync_CallsAlertServiceWithSuccessMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new CreateOperationFeedbackStrategy();
        var successMessage = "New record successfully created in DB!";

        // Act
        await strategy.ExecuteSuccessAsync(mockAlertService.Object);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(successMessage), Times.Once);
    }

    [Fact]
    public async Task ExecuteFailureAsync_CallsAlertServiceWithFailureMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new CreateOperationFeedbackStrategy();
        var failureMessage = "New record was NOT successfully created in DB!";

        // Act
        await strategy.ExecuteFailureAsync(mockAlertService.Object);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}