using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents;

public class RemoveMemberFromPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;
    private readonly RemoveMemberFromPaymentsComponent _component;

    public RemoveMemberFromPaymentsComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _mockStripeService = new Mock<ICrossCuttingStripeService>();

        _component = new RemoveMemberFromPaymentsComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLoggerService.Object);
        _component.SetPrivatePropertyValue("PaymentsService", _mockPaymentsService.Object);
        _component.SetPrivatePropertyValue("StripeService", _mockStripeService.Object);
    }

    [Fact]
    public async Task OnInitializedAsync_SubscribesToEvents()
    {
        // Arrange

        // Act
        await typeof(RemoveMemberFromPaymentsComponent).InvokeAsync("OnInitializedAsync", _component);

        // Assert
        _mockPaymentsService.VerifyAdd(m => m.RowIndexOfSelectedPaymentChanged += It.IsAny<Action>(), Times.Once);
        _mockPaymentsService.VerifyAdd(m => m.PaymentEntitiesSizeChanged += It.IsAny<Action>(), Times.Once);
    }

    [Fact]
    public void OnPaymentEntitiesSizeChanged_DisablesButtonWhenNoPayments()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);
        _mockPaymentsService.Setup(m => m.PaymentEntities).Returns(new ObservableCollection<PaymentEntity>());

        // Act
        typeof(RemoveMemberFromPaymentsComponent).Invoke("OnPaymentEntitiesSizeChanged", _component);

        // Assert
        Assert.True(_component.GetPrivatePropertyValue<bool>("DisabledBDP"));
    }

    [Fact]
    public async Task OnRemoveButtonClickedAsync_RemovesPaymentAndUpdatesState()
    {
        // Arrange
        _mockPaymentsService.Setup(m => m.RowIndexOfPayment).Returns(0);
        _mockPaymentsService.Setup(m => m.PaymentEntities).Returns(new ObservableCollection<PaymentEntity> { new() });
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await typeof(RemoveMemberFromPaymentsComponent).InvokeAsync("OnRemoveButtonClickedAsync", _component);

        // Assert
        _mockPaymentsService.Verify(m => m.OnPaymentEntitiesSizeChanged(), Times.Once);
        _mockPaymentsService.Verify(m => m.OnTitleChanged(), Times.Once);
        Assert.Equal(0, _mockStripeService.Object.StripeMemberID);
        Assert.True(_component.GetPrivatePropertyValue<bool>("DisabledBDP"));
    }

    [Fact]
    public void OnRowIndexOfSelectedPaymentChanged_EnablesButton()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        typeof(RemoveMemberFromPaymentsComponent).Invoke("OnRowIndexOfSelectedPaymentChanged", _component);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("DisabledBDP"));
    }

    [Fact]
    public void Dispose_UnsubscribesFromEvents()
    {
        // Arrange
        typeof(RemoveMemberFromPaymentsComponent).InvokeAsync("OnInitializedAsync", _component).Wait();

        // Act
        _component.Dispose();

        // Assert
        _mockPaymentsService.VerifyRemove(m => m.RowIndexOfSelectedPaymentChanged -= It.IsAny<Action>(), Times.Once);
        _mockPaymentsService.VerifyRemove(m => m.PaymentEntitiesSizeChanged -= It.IsAny<Action>(), Times.Once);
    }
}