using DataAccess.Models;
using DBExplorerBlazor.Services;
using Syncfusion.Blazor.Grids;

namespace PaymentEntityEditOperation;

public class PaymentEntityEditOperationServiceTests
{
    private readonly PaymentEntityEditOperationService _service;

    public PaymentEntityEditOperationServiceTests()
    {
        _service = new PaymentEntityEditOperationService();
    }

    [Fact]
    public void PrepareForEditOperation_ShouldCloneData()
    {
        // Arrange
        var paymentEntity = new PaymentEntity { Amount = "100", Description = "Test" };
        var arg = new ActionEventArgs<PaymentEntity> { Data = paymentEntity };
        PaymentEntity? originalData = null;
        PaymentEntity? clonedPaymentEntity = null;

        // Act
        _service.PrepareForEditOperation(arg, ref originalData, ref clonedPaymentEntity);

        // Assert
        Assert.NotNull(originalData);
        Assert.NotNull(clonedPaymentEntity);
        Assert.Equal(paymentEntity.Amount, originalData.Amount);
        Assert.Equal(paymentEntity.Description, originalData.Description);
        Assert.Equal(paymentEntity.Amount, clonedPaymentEntity.Amount);
        Assert.Equal(paymentEntity.Description, clonedPaymentEntity.Description);
        Assert.NotSame(paymentEntity, originalData);
        Assert.NotSame(paymentEntity, clonedPaymentEntity);
    }
}