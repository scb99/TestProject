using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;

namespace PaymentActionHandlerNS;

public class PaymentActionHandlerTests
{
    private readonly Mock<IPaymentEntityEditOperationService> _editOperationServiceMock;
    private readonly Mock<IPaymentEntitySaveOperationService> _saveOperationServiceMock;
    private readonly PaymentActionHandler _handler;

    public PaymentActionHandlerTests()
    {
        _editOperationServiceMock = new Mock<IPaymentEntityEditOperationService>();
        _saveOperationServiceMock = new Mock<IPaymentEntitySaveOperationService>();
        _handler = new PaymentActionHandler(_editOperationServiceMock.Object, _saveOperationServiceMock.Object);
    }

    [Fact]
    public async Task HandleActionAsync_ShouldCallPrepareForEditOperation_OnBeginEdit()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity> { RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit, Data = new PaymentEntity() };
        var gridMock = new Mock<SfGrid<PaymentEntity>>();
        var amount = "200";

        // Act
        await _handler.HandleActionAsync(arg, gridMock.Object, amount);

        // Assert
        _editOperationServiceMock.Verify(e => e.PrepareForEditOperation(arg, ref _handler.originalData, ref _handler.clonedPaymentEntity), Times.Once);
    }

    [Fact]
    public async Task HandleActionAsync_ShouldCallHandleSaveOperation_OnSave()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = new PaymentEntity() };
        var gridMock = new Mock<SfGrid<PaymentEntity>>();
        var amount = "200";

        // Act
        await _handler.HandleActionAsync(arg, gridMock.Object, amount);

        // Assert
        _saveOperationServiceMock.Verify(s => s.HandleSaveOperation(arg, ref _handler.originalData, ref _handler.clonedPaymentEntity, gridMock.Object, amount), Times.Once);
    }

    [Fact]
    public void OnValueChange_ShouldUpdateAmount()
    {
        // Arrange
        var args = new ChangeEventArgs<string, DropDownItems> { ItemData = new DropDownItems { Text = "300" } };
        var amount = "200";
        _handler.originalData = new PaymentEntity();

        // Act
        _handler.OnValueChange(args, ref amount);

        // Assert
        Assert.Equal("300", amount);
        Assert.Equal("300", _handler.originalData.Amount);
    }
}