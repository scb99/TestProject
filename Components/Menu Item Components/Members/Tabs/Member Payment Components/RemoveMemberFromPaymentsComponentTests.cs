using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents;

public class RemoveMemberFromPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;

    public RemoveMemberFromPaymentsComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
    }

    [Fact]
    public async Task OnInitializedAsync_SubscribesToEvents()
    {
        // Arrange
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var component = new RemoveMemberFromPaymentsComponent
        {
            PaymentsService = mockPaymentsService.Object,
            Logger = mockLogger.Object
        };

        // Act
        await component.OnInitialized2Async();

        // Assert
        mockPaymentsService.VerifyAdd(m => m.RowIndexOfSelectedPaymentChanged += It.IsAny<Action>(), Times.Once);
        mockPaymentsService.VerifyAdd(m => m.PaymentEntitiesSizeChanged += It.IsAny<Action>(), Times.Once);
    }

    [Fact]
    public void OnPaymentEntitiesSizeChanged_DisablesButtonWhenNoPayments()
    {
        // Arrange
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        mockPaymentsService.Setup(m => m.PaymentEntities).Returns(new ObservableCollection<PaymentEntity>());
        var component = new RemoveMemberFromPaymentsComponent
        {
            Execute = _mockExecute.Object,
            PaymentsService = mockPaymentsService.Object
        };
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        component.OnPaymentEntitiesSizeChanged();

        // Assert
        Assert.True(component.DisabledBDP);
    }

    [Fact]
    public async Task OnRemoveButtonClickedAsync_RemovesPaymentAndUpdatesState()
    {
        // Arrange
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        var mockStripeService = new Mock<ICrossCuttingStripeService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        mockPaymentsService.Setup(m => m.RowIndexOfPayment).Returns(0);
        mockPaymentsService.Setup(m => m.PaymentEntities).Returns(new ObservableCollection<PaymentEntity> { new() });

        var component = new RemoveMemberFromPaymentsComponent
        {
            Execute = _mockExecute.Object,
            PaymentsService = mockPaymentsService.Object,
            StripeService = mockStripeService.Object,
            Logger = mockLogger.Object
        };
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await component.OnRemoveButtonClickedAsync();

        // Assert
        mockPaymentsService.Verify(m => m.OnPaymentEntitiesSizeChanged(), Times.Once);
        mockPaymentsService.Verify(m => m.OnTitleChanged(), Times.Once);
        Assert.Equal(0, mockStripeService.Object.StripeMemberID);
        Assert.True(component.DisabledBDP);
    }

    [Fact]
    public void OnRowIndexOfSelectedPaymentChanged_EnablesButton()
    {
        // Arrange
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        var component = new RemoveMemberFromPaymentsComponent
        {
            Execute = _mockExecute.Object,
            PaymentsService = mockPaymentsService.Object
        };
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        component.OnRowIndexOfSelectedPaymentChanged();

        // Assert
        Assert.False(component.DisabledBDP);
    }

    [Fact]
    public void Dispose_UnsubscribesFromEvents()
    {
        // Arrange
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        var component = new RemoveMemberFromPaymentsComponent
        {
            PaymentsService = mockPaymentsService.Object
        };
        component.OnInitialized2Async().Wait();

        // Act
        component.Dispose();

        // Assert
        mockPaymentsService.VerifyRemove(m => m.RowIndexOfSelectedPaymentChanged -= It.IsAny<Action>(), Times.Once);
        mockPaymentsService.VerifyRemove(m => m.PaymentEntitiesSizeChanged -= It.IsAny<Action>(), Times.Once);
    }
}