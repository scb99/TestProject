using DataAccess.Models;
using DBExplorerBlazor.Services;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentEntityEditOperationServiceTests
{
    [Fact]
    public void PrepareForEditOperation_CreatesClonesForOriginalAndClonedData()
    {
        // Arrange
        var service = new PaymentEntityEditOperationService();
        var paymentEntity = new PaymentEntity
        {
            ID = 1,
            Description = "Test",
            Amount = "100",
            AmountDorO = "D"
        };
        var arg = new ActionEventArgs<PaymentEntity> { Data = paymentEntity };
        PaymentEntity? originalData = null;
        PaymentEntity? clonedPaymentEntity = null;

        // Act
        service.PrepareForEditOperation(arg, ref originalData, ref clonedPaymentEntity);

        // Assert
        Assert.NotNull(originalData);
        Assert.NotNull(clonedPaymentEntity);
        Assert.NotSame(arg.Data, originalData);
        Assert.NotSame(arg.Data, clonedPaymentEntity);
        Assert.NotSame(originalData, clonedPaymentEntity);
        Assert.Equal(arg.Data.ID, originalData.ID);
        Assert.Equal(arg.Data.Description, originalData.Description);
        Assert.Equal(arg.Data.Amount, originalData.Amount);
        Assert.Equal(arg.Data.AmountDorO, originalData.AmountDorO);
        Assert.Equal(arg.Data.ID, clonedPaymentEntity.ID);
        Assert.Equal(arg.Data.Description, clonedPaymentEntity.Description);
        Assert.Equal(arg.Data.Amount, clonedPaymentEntity.Amount);
        Assert.Equal(arg.Data.AmountDorO, clonedPaymentEntity.AmountDorO);
    }
}