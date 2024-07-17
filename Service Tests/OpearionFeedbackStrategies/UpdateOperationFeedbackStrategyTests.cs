using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class UpdateOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteSuccessAsync_CallsAlertServiceWithSuccessMessage()
    {
        // Arrange
        var mockAlertService = new Mock<IAlertService>();
        var strategy = new UpdateOperationFeedbackStrategy();
        var successMessage = "Selected record successfully updated in DB!";

        // Act
        await strategy.ExecuteSuccessAsync(mockAlertService.Object);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(successMessage), Times.Once);
    }

    [Fact]
    public async Task ExecuteFailureAsync_CallsAlertServiceWithFailureMessage()
    {
        // Arrange
        var mockAlertService = new Mock<IAlertService>();
        var strategy = new UpdateOperationFeedbackStrategy();
        var failureMessage = "Selected record was NOT updated in DB!";

        // Act
        await strategy.ExecuteFailureAsync(mockAlertService.Object);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}