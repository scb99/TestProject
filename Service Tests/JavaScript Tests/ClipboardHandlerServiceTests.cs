using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace JavaScript;

public class ClipboardHandlerServiceTests
{
    private readonly Mock<IClipboardService> mockClipboardService = new();
    private readonly ClipboardHandlerService clipboardHandlerService;

    public ClipboardHandlerServiceTests()
    {
        clipboardHandlerService = new ClipboardHandlerService(mockClipboardService.Object);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_CopiesFirstPaymentDetailToClipboard()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity>
        {
            new() { Description = "First Payment Detail" },
            new() { Description = "Second Payment Detail" }
        };
        var expectedDetails = paymentEntities.First().Description;

        // Act
        await clipboardHandlerService.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        mockClipboardService.Verify(c => c.CopyToClipboardAsync(expectedDetails), Times.Once);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_WithEmptyCollection_DoesNotCopyToClipboard()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity>();

        // Act
        await clipboardHandlerService.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        mockClipboardService.Verify(c => c.CopyToClipboardAsync(It.IsAny<string>()), Times.Never);
    }
}
