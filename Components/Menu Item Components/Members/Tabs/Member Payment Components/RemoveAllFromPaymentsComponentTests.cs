using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents;

public class RemoveAllFromPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
    private readonly Mock<IRemoveAllPaymentsService> _removeAllPaymentsServiceMock;
    private readonly RemoveAllFromPaymentsComponent _component;

    public RemoveAllFromPaymentsComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
        _removeAllPaymentsServiceMock = new Mock<IRemoveAllPaymentsService>();

        _component = new RemoveAllFromPaymentsComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("Logger", _loggerMock.Object);
        _component.SetPrivatePropertyValue("PaymentsService", _paymentsServiceMock.Object);
        _component.SetPrivatePropertyValue("RemoveAllPaymentsService", _removeAllPaymentsServiceMock.Object);
    }

    [Fact]
    public void OnInitialized_SubscribesToPaymentEntitiesSizeChanged()
    {
        // Act
        typeof(RemoveAllFromPaymentsComponent).Invoke("OnInitialized", _component);

        // Assert
        _paymentsServiceMock.VerifyAdd(p => p.PaymentEntitiesSizeChanged += It.IsAny<Action>(), Times.Once);
    }

    [Fact]
    public void OnPaymentEntitiesSizeChanged_UpdatesDisabledBDP()
    {
        // Arrange
        ObservableCollection<PaymentEntity> temp = new();
        _paymentsServiceMock.Setup(p => p.PaymentEntities).Returns(temp);
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        typeof(RemoveAllFromPaymentsComponent).Invoke("OnPaymentEntitiesSizeChanged", _component);

        // Assert
        Assert.True(_component.GetPrivatePropertyValue<bool>("DisabledBDP"));
    }

    [Fact]
    public async Task OnRemoveAllButtonClickedAsync_CallsRemoveAllPaymentsAsync()
    {
        // Act
        await typeof(RemoveAllFromPaymentsComponent).InvokeAsync("OnRemoveAllButtonClickedAsync", _component);

        // Assert
        _removeAllPaymentsServiceMock.Verify(r => r.RemoveAllPaymentsAsync(), Times.Once);
    }

    [Fact]
    public void Dispose_UnsubscribesFromPaymentEntitiesSizeChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _paymentsServiceMock.VerifyRemove(p => p.PaymentEntitiesSizeChanged -= It.IsAny<Action>(), Times.Once);
    }
}
