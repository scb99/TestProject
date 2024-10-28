using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentsComponentTests
{
    private readonly Mock<ICrossCuttingLoggerService> mockLogger = new();
    private readonly Mock<IPaymentActionHandlerService> mockPaymentActionHandler = new();
    private readonly Mock<ICrossCuttingPaymentsService> mockPaymentsService = new();
    private readonly Mock<ICrossCuttingPaymentToMembershipIDService> mockPaymentToMembershipIDService = new();

    [Fact]
    public async Task OnActionBeginAsync_CallsHandleActionAsync()
    {
        // Arrange
        var component = new PaymentsComponent
        {
            Logger = mockLogger.Object,
            PaymentActionHandler = mockPaymentActionHandler.Object,
            PaymentsService = mockPaymentsService.Object,
            PaymentToMembershipIDService = mockPaymentToMembershipIDService.Object
        };
        var arg = new ActionEventArgs<PaymentEntity>();

        // Act
        await component.OnActionBeginAsync(arg);

        // Assert
        mockPaymentActionHandler.Verify(handler => handler.HandleActionAsync(arg, It.IsAny<SfGrid<PaymentEntity>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnSelectedRowChangedAsync_HandlesException()
    {
        // Arrange
        var component = new PaymentsComponent
        {
            Logger = mockLogger.Object,
            PaymentActionHandler = mockPaymentActionHandler.Object,
            PaymentsService = mockPaymentsService.Object,
            PaymentToMembershipIDService = mockPaymentToMembershipIDService.Object
        };
        var args = new RowSelectEventArgs<PaymentEntity> { RowIndex = 1 };
        mockPaymentsService.Setup(service => service.OnRowIndexOfSelectedPaymentChanged()).Throws(new Exception());

        // Act
        await component.OnSelectedRowChangedAsync(args);

        // Assert
        mockLogger.Verify(logger => logger.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsItems()
    {
        // Arrange
        var expectedItems = new System.Collections.Generic.List<DropDownItems> { new() { Text = "Test", ID = "1" } };
        mockPaymentToMembershipIDService.Setup(service => service.GetPaymentToMembershipIDDropDownListAsync()).ReturnsAsync(expectedItems);
        var component = new PaymentsComponent
        {
            Logger = mockLogger.Object,
            PaymentActionHandler = mockPaymentActionHandler.Object,
            PaymentsService = mockPaymentsService.Object,
            PaymentToMembershipIDService = mockPaymentToMembershipIDService.Object
        };

        // Act
        await component.OnParametersSet2Async();

        // Assert
        Assert.Equal(expectedItems, component._items);
    }
}