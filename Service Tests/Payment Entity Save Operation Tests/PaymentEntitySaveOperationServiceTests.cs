using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;
using System.Collections.ObjectModel;

namespace PaymentEntitySaveOperation;

public class PaymentEntitySaveOperationServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _alertServiceMock;
    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
    private readonly PaymentEntitySaveOperationService _service;

    public PaymentEntitySaveOperationServiceTests()
    {
        _alertServiceMock = new Mock<ICrossCuttingAlertService>();
        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
        _service = new PaymentEntitySaveOperationService(_alertServiceMock.Object, _paymentsServiceMock.Object);
    }

    [Fact]
    public void IsDataDirty_ShouldReturnTrue_WhenDataIsDirty()
    {
        // Arrange
        var current = new PaymentEntity { Amount = "100", Description = "D", AmountDorO = "50" };
        var cloned = new PaymentEntity { Amount = "100", Description = "D", AmountDorO = "40" };

        // Act
        var result = PaymentEntitySaveOperationService.IsDataDirty(current, cloned);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDataDirty_ShouldReturnFalse_WhenDataIsNotDirty()
    {
        // Arrange
        var current = new PaymentEntity { Amount = "100", Description = "D", AmountDorO = "50" };
        var cloned = new PaymentEntity { Amount = "100", Description = "D", AmountDorO = "50" };

        // Act
        var result = PaymentEntitySaveOperationService.IsDataDirty(current, cloned);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RetrieveAmount_ShouldReturnAmount_WhenDescriptionIsR()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity> { Data = new PaymentEntity { Description = "R" } };
        var amount = "200";

        // Act
        var result = _service.RetrieveAmount(arg, amount);

        // Assert
        Assert.Equal(amount, result);
        Assert.Equal("O", arg.Data.AmountDorO);
    }

    [Fact]
    public void RetrieveAmount_ShouldSetAmountToZero_WhenAmountIsNegative()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity> { Data = new PaymentEntity { AmountDorO = "-100" } };

        // Act
        var result = _service.RetrieveAmount(arg, "0");

        // Assert
        Assert.Equal("0", result);
        _alertServiceMock.Verify(a => a.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void HandleSaveOperation_ShouldUpdatePaymentEntityAndTriggerEvents()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity> { Data = new PaymentEntity { Description = "D", AmountDorO = "100" } };
        var originalData = new PaymentEntity { Amount = "50", Description = "O" };
        var clonedPaymentEntity = new PaymentEntity { Amount = "50", Description = "O" };
        var gridMock = new Mock<SfGrid<PaymentEntity>>();
        _paymentsServiceMock.SetupGet(p => p.PaymentEntities).Returns(new ObservableCollection<PaymentEntity> { new() });
        _paymentsServiceMock.SetupGet(p => p.RowIndexOfPayment).Returns(0);

        // Act
        _service.HandleSaveOperation(arg, ref originalData, ref clonedPaymentEntity, gridMock.Object, "200");

        // Assert
        Assert.Equal("100", originalData.Amount);
        Assert.Equal(originalData, clonedPaymentEntity);
        _paymentsServiceMock.Verify(p => p.OnTitleChanged(), Times.Once);
        //gridMock.Verify(g => g.Refresh(), Times.Once);
    }
}