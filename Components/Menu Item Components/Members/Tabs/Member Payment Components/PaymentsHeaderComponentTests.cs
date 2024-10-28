using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class PaymentsHeaderComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;

    public PaymentsHeaderComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
    }

    [Fact]
    public void OnTotalAmountChanged_UpdatesPaymentTitleBDP()
    {
        // Arrange
        var mockPaymentTotalService = new Mock<IPaymentTotalService>();

        var component = new PaymentsHeaderComponent
        {
            PaymentsTotalService = mockPaymentTotalService.Object,
            Execute = _mockExecute.Object
        };
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Simulate the component's OnInitialized method
        component.OnInitialized2();

        var expectedTotal = 123.45;

        // Act
        mockPaymentTotalService.Raise(m => m.TotalAmountChanged += null, expectedTotal);

        // Assert
        Assert.Equal($"Total: ${expectedTotal:F2}", component.PaymentTitleBDP);
    }

    [Fact]
    public void OnTotalAmountChanged2_UpdatesPaymentTitleBDP()
    {
        // Arrange
        var mockPaymentTotalService = new Mock<IPaymentTotalService>();
        var component = new PaymentsHeaderComponent
        {
            PaymentsTotalService = mockPaymentTotalService.Object,
            Execute = _mockExecute.Object
        };
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);
        var totalAmount = 123.45;

        // Act
        component.OnTotalAmountChanged(totalAmount);

        // Assert
        Assert.Equal($"Total: ${totalAmount:F2}", component.PaymentTitleBDP);
    }

    [Fact]
    public void Dispose_ShouldDetachFromTotalAmountChangedEvent()
    {
        // Arrange
        var mockPaymentTotalService = new Mock<IPaymentTotalService>();
        var wasEventDetached = false;

        mockPaymentTotalService.SetupAdd(service => service.TotalAmountChanged += It.IsAny<Action<double>>());
        mockPaymentTotalService.SetupRemove(service => service.TotalAmountChanged -= It.IsAny<Action<double>>())
                               .Callback(() => wasEventDetached = true);

        var component = new PaymentsHeaderComponent
        {
            PaymentsTotalService = mockPaymentTotalService.Object
        };

        // Act
        component.Dispose();

        // Assert
        Assert.True(wasEventDetached, "The TotalAmountChanged event handler was not properly detached upon disposal.");
    }
}