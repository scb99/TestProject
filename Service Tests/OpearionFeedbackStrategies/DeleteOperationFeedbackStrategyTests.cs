using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class DeleteOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteSuccessAsync_CallsAlertServiceWithSuccessMessage()
    {
        // Arrange
        var mockAlertService = new Mock<IAlertService>();
        var strategy = new DeleteOperationFeedbackStrategy();
        var successMessage = "Selected record successfully deleted from DB!";

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
        var strategy = new DeleteOperationFeedbackStrategy();
        var failureMessage = "Selected record was NOT deleted from DB!";

        // Act
        await strategy.ExecuteFailureAsync(mockAlertService.Object);

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}