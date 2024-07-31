using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class PaymentsServiceTests
{
    private readonly Mock<ICrossCuttingStripeService> _stripeServiceMock;
    private readonly PaymentsService _service;

    public PaymentsServiceTests()
    {
        _stripeServiceMock = new Mock<ICrossCuttingStripeService>();
        _service = new PaymentsService(_stripeServiceMock.Object);
    }

    [Fact]
    public void AddPaymentToPaymentEntities_ShouldAddPaymentAndTriggerEvents()
    {
        // Arrange
        var paymentEntity = new PaymentEntity();
        bool paymentEntitiesSizeChangedTriggered = false;
        bool titleChangedTriggered = false;

        _service.PaymentEntitiesSizeChanged += () => paymentEntitiesSizeChangedTriggered = true;
        _service.TitleChanged += () => titleChangedTriggered = true;

        // Act
        _service.AddPaymentToPaymentEntities(paymentEntity);

        // Assert
        Assert.Contains(paymentEntity, _service.PaymentEntities);
        Assert.True(paymentEntitiesSizeChangedTriggered);
        Assert.True(titleChangedTriggered);
    }

    [Fact]
    public void RemoveAllPaymentsFromPaymentEntities_ShouldClearPaymentsAndTriggerEvents()
    {
        // Arrange
        var paymentEntity = new PaymentEntity();
        _service.PaymentEntities.Add(paymentEntity);
        bool paymentEntitiesSizeChangedTriggered = false;
        bool titleChangedTriggered = false;

        _service.PaymentEntitiesSizeChanged += () => paymentEntitiesSizeChangedTriggered = true;
        _service.TitleChanged += () => titleChangedTriggered = true;

        // Act
        _service.RemoveAllPaymentsFromPaymentEntities();

        // Assert
        Assert.Empty(_service.PaymentEntities);
        Assert.True(paymentEntitiesSizeChangedTriggered);
        Assert.True(titleChangedTriggered);
        _stripeServiceMock.VerifySet(s => s.StripeMemberID = 0, Times.Once);
    }

    [Fact]
    public void OnRowIndexOfSelectedPaymentChanged_ShouldTriggerEvent()
    {
        // Arrange
        bool rowIndexOfSelectedPaymentChangedTriggered = false;
        _service.RowIndexOfSelectedPaymentChanged += () => rowIndexOfSelectedPaymentChangedTriggered = true;

        // Act
        _service.OnRowIndexOfSelectedPaymentChanged();

        // Assert
        Assert.True(rowIndexOfSelectedPaymentChangedTriggered);
    }
}