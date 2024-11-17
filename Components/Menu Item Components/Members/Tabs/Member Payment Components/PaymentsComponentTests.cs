using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentsComponentTests
{
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IPaymentActionHandlerService> _mockPaymentActionHandler;
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly Mock<ICrossCuttingPaymentToMembershipIDService> _mockPaymentToMembershipIDService;
    private readonly PaymentsComponent _component;


    public PaymentsComponentTests()
    {
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockPaymentActionHandler = new Mock<IPaymentActionHandlerService>();
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _mockPaymentToMembershipIDService = new Mock<ICrossCuttingPaymentToMembershipIDService>();

        _component = new PaymentsComponent();

        _component.SetPrivatePropertyValue("Logger", _mockLogger.Object);
        _component.SetPrivatePropertyValue("PaymentActionHandler", _mockPaymentActionHandler.Object);
        _component.SetPrivatePropertyValue("PaymentsService", _mockPaymentsService.Object);
        _component.SetPrivatePropertyValue("PaymentToMembershipIDService", _mockPaymentToMembershipIDService.Object);
    }

    [Fact]
    public async Task OnActionBeginAsync_CallsHandleActionAsync()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity>();

        // Act
        await _component.OnActionBeginAsync(arg);

        // Assert
        _mockPaymentActionHandler.Verify(handler => handler.HandleActionAsync(arg, It.IsAny<SfGrid<PaymentEntity>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnSelectedRowChangedAsync_HandlesException()
    {
        // Arrange
        var args = new RowSelectEventArgs<PaymentEntity> { RowIndex = 1 };
        _mockPaymentsService.Setup(service => service.OnRowIndexOfSelectedPaymentChanged()).Throws(new Exception());

        // Act
        await typeof(PaymentsComponent).InvokeAsync("OnSelectedRowChangedAsync", _component, args);

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsItems()
    {
        // Arrange
        var expectedItems = new System.Collections.Generic.List<DropDownItems> { new() { Text = "Test", ID = "1" } };
        _mockPaymentToMembershipIDService.Setup(service => service.GetPaymentToMembershipIDDropDownListAsync()).ReturnsAsync(expectedItems);

        // Act
        await typeof(PaymentsComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(expectedItems, _component.GetPrivateMemberValue<List<DropDownItems>>("_items"));
    }
}