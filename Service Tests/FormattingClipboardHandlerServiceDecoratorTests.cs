using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class FormattingClipboardHandlerServiceDecoratorTests
{
    private readonly Mock<IClipboardHandler> mockWrappedHandler = new();
    private readonly Mock<IPaymentDetailsFormatter> mockFormatter = new();
    private readonly FormattingClipboardHandlerServiceDecorator decorator;

    public FormattingClipboardHandlerServiceDecoratorTests()
    {
        decorator = new FormattingClipboardHandlerServiceDecorator(mockWrappedHandler.Object, mockFormatter.Object);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_FormatsAndPassesDetailsToWrappedHandler()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity>
        {
            new() { Description = "Payment 1" },
            new() { Description = "Payment 2" }
        };
        var formattedDetails = "Formatted Details";
        mockFormatter.Setup(f => f.FormatForClipboard(paymentEntities)).Returns(formattedDetails);

        // Act
        await decorator.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        mockFormatter.Verify(f => f.FormatForClipboard(paymentEntities), Times.Once);
        mockWrappedHandler.Verify(h => h.CopyPaymentDetailsToClipboardAsync(It.Is<IEnumerable<PaymentEntity>>(e => e.Any(pe => pe.Description == formattedDetails))), Times.Once);
    }
}
