using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentActionHandlerTests
{
    private readonly Mock<ILoggerService> _mockLoggerService = new();
    private readonly Mock<IPaymentEntityEditOperationService> _mockEditOperationService = new();
    private readonly Mock<IPaymentEntitySaveOperationService> _mockSaveOperationService = new();
    private readonly PaymentActionHandler _paymentActionHandler;

    public PaymentActionHandlerTests()
    {
        _paymentActionHandler = new PaymentActionHandler(
            _mockLoggerService.Object,
            _mockEditOperationService.Object,
            _mockSaveOperationService.Object);
    }

    [Fact]
    public async Task HandleActionAsync_BeginEdit_CallsEditOperationService()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PaymentEntity>
        {
            RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit
        };
        var grid = new Mock<SfGrid<PaymentEntity>>().Object;
        string amount = "100";

        // Act
        await _paymentActionHandler.HandleActionAsync(actionArgs, grid, amount);

        // Assert
        _mockEditOperationService.Verify(s => s.PrepareForEditOperation(It.IsAny<ActionEventArgs<PaymentEntity>>(), ref It.Ref<PaymentEntity>.IsAny, ref It.Ref<PaymentEntity>.IsAny), Times.Once);
    }

    [Fact]
    public async Task HandleActionAsync_Save_CallsSaveOperationService()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PaymentEntity>
        {
            RequestType = Syncfusion.Blazor.Grids.Action.Save
        };
        var grid = new Mock<SfGrid<PaymentEntity>>().Object;
        string amount = "100";

        // Act
        await _paymentActionHandler.HandleActionAsync(actionArgs, grid, amount);

        // Assert
        _mockSaveOperationService.Verify(s => s.HandleSaveOperation(It.IsAny<ActionEventArgs<PaymentEntity>>(), ref It.Ref<PaymentEntity>.IsAny, ref It.Ref<PaymentEntity>.IsAny, It.IsAny<SfGrid<PaymentEntity>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleActionAsync_ThrowsException_LogsException()
    {
        // Arrange
        var actionArgs = new ActionEventArgs<PaymentEntity>
        {
            RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit
        };
        var grid = new Mock<SfGrid<PaymentEntity>>().Object;
        string amount = "100";
        _mockEditOperationService.Setup(s => s.PrepareForEditOperation(It.IsAny<ActionEventArgs<PaymentEntity>>(), ref It.Ref<PaymentEntity>.IsAny, ref It.Ref<PaymentEntity>.IsAny))
            .Throws(new Exception("Test exception"));

        // Act
        await _paymentActionHandler.HandleActionAsync(actionArgs, grid, amount);

        // Assert
        _mockLoggerService.Verify(s => s.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}