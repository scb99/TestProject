using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
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
        _component = new RemoveAllFromPaymentsComponent
        {
            Execute = _mockExecute.Object,
            Logger = _loggerMock.Object,
            PaymentsService = _paymentsServiceMock.Object,
            RemoveAllPaymentsService = _removeAllPaymentsServiceMock.Object
        };
    }

    [Fact]
    public void OnInitialized_SubscribesToPaymentEntitiesSizeChanged()
    {
        // Act
        _component.OnInitialized2();

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
        _component.OnPaymentEntitiesSizeChanged();

        // Assert
        Assert.True(_component.DisabledBDP);
    }

    [Fact]
    public async Task OnRemoveAllButtonClickedAsync_CallsRemoveAllPaymentsAsync()
    {
        // Act
        await _component.OnRemoveAllButtonClickedAsync();

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
