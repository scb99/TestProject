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
        var strategy = new CreateOperationFeedbackStrategyService(mockAlertService.Object);
        var successMessage = "New record successfully created in DB!";

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
        var strategy = new CreateOperationFeedbackStrategyService(mockAlertService.Object);
        var failureMessage = "New record was NOT successfully created in DB!";

        // Act
        await strategy.ExecuteFailureAsync();

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}