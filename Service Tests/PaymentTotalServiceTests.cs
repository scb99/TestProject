using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MenuItemComponents;

public class PaymentTotalServiceTests
{
    private readonly Mock<IPaymentsService> _mockPaymentsService;
    private readonly PaymentTotalService _paymentTotalService;

    public PaymentTotalServiceTests()
    {
        _mockPaymentsService = new Mock<IPaymentsService>();
        _paymentTotalService = new PaymentTotalService(_mockPaymentsService.Object);
    }

    [Fact]
    public void UpdateTotalAmount_WhenInvoked_FiresTotalAmountChangedEventWithCorrectTotal()
    {
        // Arrange
        var paymentEntities = new System.Collections.ObjectModel.ObservableCollection<PaymentEntity>
        {
            new() { Amount = "100" },
            new() { Amount = "200" }
        };
        _mockPaymentsService.Setup(service => service.PaymentEntities).Returns(paymentEntities);

        double? capturedTotal = null;
        _paymentTotalService.TotalAmountChanged += (total) => capturedTotal = total;

        // Act
        _paymentTotalService.UpdateTotalAmount();

        // Assert
        Assert.NotNull(capturedTotal);
        Assert.Equal(300, capturedTotal);
    }

    [Fact]
    public void Constructor_SubscribesToTitleChangedEventOnPaymentsService()
    {
        // Arrange
        var wasEventSubscribed = false;
        _mockPaymentsService.SetupAdd(service => service.TitleChanged += It.IsAny<Action>())
            .Callback(() => wasEventSubscribed = true);

        // Act
        var service = new PaymentTotalService(_mockPaymentsService.Object);

        // Assert
        Assert.True(wasEventSubscribed);
    }

    [Fact]
    public void Dispose_UnsubscribesFromTitleChangedEventOnPaymentsService()
    {
        // Arrange
        var wasEventUnsubscribed = false;
        _mockPaymentsService.SetupRemove(service => service.TitleChanged -= It.IsAny<Action>())
            .Callback(() => wasEventUnsubscribed = true);

        // Act
        _paymentTotalService.Dispose();

        // Assert
        Assert.True(wasEventUnsubscribed);
    }
}