using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using System.Collections.ObjectModel;

namespace MemberPayment;

public class MemberPaymentClipboardServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<IClipboardService> _mockClipboardService;
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly MemberPaymentClipboardService _service;

    public MemberPaymentClipboardServiceTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockClipboardService = new Mock<IClipboardService>();
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _service = new MemberPaymentClipboardService(
            _mockAlertService.Object,
            _mockClipboardService.Object,
            _mockPaymentsService.Object);
    }

    [Fact]
    public void PrepareClipboardList_ShouldReturnCorrectList()
    {
        // Arrange
        ObservableCollection<PaymentEntity> paymentEntities = new()
        {
            new() { ID = 1, Description = "Desc1", FirstName = "John", LastName = "Doe" },
            new() { ID = 2, Description = "Desc2", FirstName = "Jane", LastName = "Smith" }
        };
        _mockPaymentsService.Setup(p => p.PaymentEntities).Returns(paymentEntities);

        // Act
        var result = _service.PrepareClipboardList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains("1 Desc1 Doe, John", result);
        Assert.Contains("2 Desc2 Smith, Jane", result);
        Assert.Contains("John Doe, Jane Smith renewal", result);
    }

    [Fact]
    public async Task SendNextItemToClipboardAsync_ShouldAlertWhenListIsEmpty()
    {
        // Arrange
        var clipboardList = new List<string>();

        // Act
        var result = await _service.SendNextItemToClipboardAsync(clipboardList, 0);

        // Assert
        Assert.Equal(0, result);
        _mockAlertService.Verify(a => a.AlertUsingFallingMessageBoxAsync("No data to copy to clipboard!"), Times.Once);
    }

    [Fact]
    public async Task SendNextItemToClipboardAsync_ShouldCopyNextItemToClipboard()
    {
        // Arrange
        var clipboardList = new List<string> { "Item1", "Item2" };

        // Act
        var result = await _service.SendNextItemToClipboardAsync(clipboardList, 0);

        // Assert
        Assert.Equal(1, result);
        _mockClipboardService.Verify(c => c.CopyToClipboardAsync("Item1"), Times.Once);
        _mockAlertService.Verify(a => a.AlertUsingFallingMessageBoxAsync("Added \"Item1\" to Clipboard!"), Times.Once);
    }

    [Fact]
    public async Task SendNextItemToClipboardAsync_ShouldCycleThroughList()
    {
        // Arrange
        var clipboardList = new List<string> { "Item1", "Item2" };

        // Act
        var result = await _service.SendNextItemToClipboardAsync(clipboardList, 2);

        // Assert
        Assert.Equal(1, result);
        _mockClipboardService.Verify(c => c.CopyToClipboardAsync("Item1"), Times.Once);
        _mockAlertService.Verify(a => a.AlertUsingFallingMessageBoxAsync("Added \"Item1\" to Clipboard!"), Times.Once);
    }
}

// Mock PaymentEntity class for testing purposes
//public class PaymentEntity
//{
//    public int ID { get; set; }
//    public string? Description { get; set; }
//    public string? FirstName { get; set; }
//    public string? LastName { get; set; }
//}
