using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class PaymentsHeaderComponentTests
{
    [Fact]
    public void OnTotalAmountChanged_UpdatesPaymentTitleBDP()
    {
        // Arrange
        var mockPaymentTotalService = new Mock<IPaymentTotalService>();

        var component = new PaymentsHeaderComponent
        {
            PaymentsTotalService = mockPaymentTotalService.Object
        };

        // Simulate the component's OnInitialized method
        component.OnInitialized2();

        var expectedTotal = 123.45;

        // Act
        mockPaymentTotalService.Raise(m => m.TotalAmountChanged += null, expectedTotal);

        // Assert
        Assert.Equal($"Total: ${expectedTotal:F2}", component.PaymentTitleBDP);
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