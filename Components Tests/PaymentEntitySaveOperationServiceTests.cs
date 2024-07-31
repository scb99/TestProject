using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentEntitySaveOperationServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService = new();
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService = new();
    private readonly PaymentEntitySaveOperationService _service;

    public PaymentEntitySaveOperationServiceTests()
    {
        _service = new PaymentEntitySaveOperationService(_mockAlertService.Object, _mockPaymentsService.Object);
    }

    [Fact]
    public void HandleSaveOperation_WhenDescriptionIsDOrO_UpdatesOriginalDataAmount()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity>
        {
            Data = new PaymentEntity { Description = "D", AmountDorO = "100" }
        };
        var originalData = new PaymentEntity();
        var clonedPaymentEntity = new PaymentEntity();
        var grid = new Mock<SfGrid<PaymentEntity>>();
        string amount = "50";

        var temp = new System.Collections.ObjectModel.ObservableCollection<PaymentEntity>
        {
            new() { Description = "D", AmountDorO = "100" }
        };

        _mockPaymentsService.SetupGet(s => s.PaymentEntities).Returns(temp);
        _mockPaymentsService.SetupGet(s => s.RowIndexOfPayment).Returns(0);

        // Act
        _service.HandleSaveOperation(arg, ref originalData, ref clonedPaymentEntity, grid.Object, amount);

        // Assert
        Assert.Equal("100", originalData.Amount);
    }

    [Fact]
    public void HandleSaveOperation_WhenDataIsDirty_UpdatesPaymentServiceAndCallsOnTitleChanged()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        var paymentEntities = new System.Collections.ObjectModel.ObservableCollection<PaymentEntity>
        {
            new() { Amount = "50", Description = "Initial" }
        };
        mockPaymentsService.SetupGet(s => s.PaymentEntities).Returns(paymentEntities);
        mockPaymentsService.SetupGet(s => s.RowIndexOfPayment).Returns(0);

        var service = new PaymentEntitySaveOperationService(mockAlertService.Object, mockPaymentsService.Object);

        var arg = new ActionEventArgs<PaymentEntity>
        {
            Data = new PaymentEntity { Description = "D", AmountDorO = "100" }
        };
        var originalData = new PaymentEntity { Amount = "50", Description = "Initial" };
        var clonedPaymentEntity = new PaymentEntity { Amount = "60", Description = "Changed" }; // Different to simulate dirty data
        var grid = new Mock<SfGrid<PaymentEntity>>().Object; // Not used in the test due to IsMock check
        string amount = "100";

        // Act
        service.HandleSaveOperation(arg, ref originalData, ref clonedPaymentEntity, grid, amount);

        // Assert
        Assert.Equal("100", paymentEntities[0].Amount); // Assert that the amount was updated
        Assert.Equal("D", paymentEntities[0].Description); // Assert that the description was updated
        mockPaymentsService.Verify(m => m.OnTitleChanged(), Times.Once); // Verify that OnTitleChanged was called
    }

    [Theory]
    [InlineData("R", "O", "100")] // Description R, AmountDorO O, expected amount 100
    [InlineData("Invalid", null, "0")] // Invalid description, null AmountDorO, expected amount 0
    public void RetrieveAmount_VariousConditions_ReturnsExpectedAmount(string description, string amountDorO, string expectedAmount)
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentEntity>
        {
            Data = new PaymentEntity { Description = description, AmountDorO = amountDorO }
        };
        string amount = "100";

        // Act
        var result = _service.RetrieveAmount(arg, amount); // Assuming you make RetrieveAmount public or internal for testing, or use reflection to access it.

        // Assert
        Assert.Equal(expectedAmount, result);
    }
}