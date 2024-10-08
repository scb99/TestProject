﻿using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CRUDOperationFeedbackStrategy;

public class DeleteOperationFeedbackStrategyTests
{
    [Fact]
    public async Task ExecuteSuccessAsync_CallsAlertServiceWithSuccessMessage()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var strategy = new DeleteOperationFeedbackStrategy(mockAlertService.Object);
        var successMessage = "Selected record successfully deleted from DB!";

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
        var strategy = new DeleteOperationFeedbackStrategy(mockAlertService.Object);
        var failureMessage = "Selected record was NOT deleted from DB!";

        // Act
        await strategy.ExecuteFailureAsync();

        // Assert
        mockAlertService.Verify(service => service.AlertUsingFallingMessageBoxAsync(failureMessage), Times.Once);
    }
}