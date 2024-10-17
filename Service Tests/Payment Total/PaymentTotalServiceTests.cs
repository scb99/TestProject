using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using System.Collections.ObjectModel;

namespace PaymentTotal;

public class PaymentTotalServiceTests
{
    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
    private readonly PaymentTotalService _service;

    public PaymentTotalServiceTests()
    {
        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
        _service = new PaymentTotalService(_paymentsServiceMock.Object);
    }

    [Fact]
    public void UpdateTotalAmount_CalculatesTotalAmountCorrectly()
    {
        // Arrange
        var paymentEntities = new ObservableCollection<PaymentEntity>
        {
            new() { Amount = "100" },
            new() { Amount = "200" },
            new() { Amount = "300" }
        };
        _paymentsServiceMock.Setup(s => s.PaymentEntities).Returns(paymentEntities);
        double expectedTotal = paymentEntities.Sum(pe => Convert.ToDouble(pe.Amount));
        double actualTotal = 0;
        _service.TotalAmountChanged += total => actualTotal = total;

        // Act
        _service.UpdateTotalAmount();

        // Assert
        Assert.Equal(expectedTotal, actualTotal);
    }

    [Fact]
    public void Dispose_UnsubscribesFromTitleChangedEvent()
    {
        // Act
        _service.Dispose();

        // Assert
        _paymentsServiceMock.VerifyRemove(s => s.TitleChanged -= _service.UpdateTotalAmount, Times.Once);
    }
}