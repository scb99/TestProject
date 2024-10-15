using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentActionHandlerTests
{
    [Fact]
    public async Task HandleActionAsync_Calls_PrepareForEditOperation_OnBeginEdit()
    {
        // Arrange
        var editServiceMock = new Mock<IPaymentEntityEditOperationService>();
        var saveServiceMock = new Mock<IPaymentEntitySaveOperationService>();
        var handler = new PaymentActionHandler(editServiceMock.Object, saveServiceMock.Object);
        var args = new Syncfusion.Blazor.Grids.ActionEventArgs<PaymentEntity> { RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit };
        var grid = new SfGrid<PaymentEntity>();
        string amount = "100";

        // Act
        await handler.HandleActionAsync(args, grid, amount);

        // Assert
        editServiceMock.Verify(e => e.PrepareForEditOperation(args, ref It.Ref<PaymentEntity>.IsAny, ref It.Ref<PaymentEntity>.IsAny), Times.Once);
    }

    [Fact]
    public async Task HandleActionAsync_Calls_HandleSaveOperation_OnSave()
    {
        // Arrange
        var editServiceMock = new Mock<IPaymentEntityEditOperationService>();
        var saveServiceMock = new Mock<IPaymentEntitySaveOperationService>();
        var handler = new PaymentActionHandler(editServiceMock.Object, saveServiceMock.Object);
        var args = new ActionEventArgs<PaymentEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save };
        var grid = new SfGrid<PaymentEntity>();
        string amount = "100";

        // Act
        await handler.HandleActionAsync(args, grid, amount);

        // Assert
        saveServiceMock.Verify(s => s.HandleSaveOperation(args, ref It.Ref<PaymentEntity>.IsAny, ref It.Ref<PaymentEntity>.IsAny, grid, amount), Times.Once);
    }

    [Fact]
    public void OnValueChange_Updates_Amount_Correctly()
    {
        // Arrange
        var editServiceMock = new Mock<IPaymentEntityEditOperationService>();
        var saveServiceMock = new Mock<IPaymentEntitySaveOperationService>();
        var handler = new PaymentActionHandler(editServiceMock.Object, saveServiceMock.Object)
        {
            originalData = new PaymentEntity { Amount = "100" }
        };
        var args = new ChangeEventArgs<string, DropDownItems> { ItemData = new DropDownItems { Text = "200" } };
        string amount = "100";

        // Act
        handler.OnValueChange(args, ref amount);

        // Assert
        Assert.Equal("200", amount);
    }
}