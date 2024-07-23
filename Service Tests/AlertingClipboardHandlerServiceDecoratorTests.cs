using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class AlertingClipboardHandlerServiceDecoratorTests
{
    private readonly Mock<IAlertService> mockAlertService = new();
    private readonly Mock<IClipboardHandler> mockWrappedHandler = new();
    private readonly AlertingClipboardHandlerServiceDecorator decorator;

    public AlertingClipboardHandlerServiceDecoratorTests()
    {
        decorator = new AlertingClipboardHandlerServiceDecorator(mockAlertService.Object, mockWrappedHandler.Object);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_CallsWrappedHandlerAndThenAlertsUser()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity> { new() { Description = "Test Payment" } };

        // Act
        await decorator.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        mockWrappedHandler.Verify(h => h.CopyPaymentDetailsToClipboardAsync(paymentEntities), Times.Once);
        mockAlertService.Verify(a => a.AlertUsingFallingMessageBoxAsync("Added payments to Clipboard!"), Times.Once);
    }
}