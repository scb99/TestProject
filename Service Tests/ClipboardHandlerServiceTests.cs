using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class ClipboardHandlerServiceTests
{
    private readonly Mock<IClipboardService> _mockClipboardService;
    private readonly Mock<IAlertService> _mockAlertService;
    private readonly ClipboardHandlerService _clipboardHandlerService;

    public ClipboardHandlerServiceTests()
    {
        _mockClipboardService = new Mock<IClipboardService>();
        _mockAlertService = new Mock<IAlertService>();
        _clipboardHandlerService = new ClipboardHandlerService(_mockClipboardService.Object, _mockAlertService.Object);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_ShouldCopyPaymentDetailsToClipboard()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, Description = "Payment 1", FirstName = "John", LastName = "Doe", Amount = "100" },
            new() { ID = 2, Description = "Payment 2", FirstName = "Jane", LastName = "Smith", Amount = "200" }
        };

        // Act
        await _clipboardHandlerService.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        _mockClipboardService.Verify(cs => cs.CopyToClipboardAsync(It.IsAny<string>()), Times.Once);
        _mockAlertService.Verify(als => als.AlertUsingFallingMessageBoxAsync("Added payments to Clipboard!"), Times.Once);
    }

    [Fact]
    public async Task CopyPaymentDetailsToClipboardAsync_ShouldFormatPaymentDetailsCorrectly()
    {
        // Arrange
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, Description = "Payment 1", FirstName = "John", LastName = "Doe", Amount = "100" },
            new() { ID = 2, Description = "Payment 2", FirstName = "Jane", LastName = "Smith", Amount = "200" }
        };

        var expectedClipboardText = "1 Payment 1 Doe, John 100" + System.Environment.NewLine +
                                    "2 Payment 2 Smith, Jane 200" + System.Environment.NewLine +
                                    "John Doe, Jane Smith renewal";

        // Act
        await _clipboardHandlerService.CopyPaymentDetailsToClipboardAsync(paymentEntities);

        // Assert
        _mockClipboardService.Verify(cs => cs.CopyToClipboardAsync(expectedClipboardText), Times.Once);
    }
}