using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;

namespace MenuItemComponents;

public class PaymentsHeaderComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<IPaymentTotalService> _mockPaymentTotalService;
    private readonly PaymentsHeaderComponent _component;

    public PaymentsHeaderComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockPaymentTotalService = new Mock<IPaymentTotalService>();

        _component = new PaymentsHeaderComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("PaymentsTotalService", _mockPaymentTotalService.Object);
    }

    [Fact]
    public void OnTotalAmountChanged_UpdatesPaymentTitleBDP()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Simulate the component's OnInitialized method
        typeof(PaymentsHeaderComponent).Invoke("OnInitialized", _component);

        var expectedTotal = 123.45;

        // Act
        _mockPaymentTotalService.Raise(m => m.TotalAmountChanged += null, expectedTotal);

        // Assert
        Assert.Equal($"Total: ${expectedTotal:F2}", _component.GetPrivatePropertyValue<string>("PaymentTitleBDP"));
    }

    [Fact]
    public void OnTotalAmountChanged2_UpdatesPaymentTitleBDP()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);
        var totalAmount = 123.45;

        // Act
        typeof(PaymentsHeaderComponent).Invoke("OnTotalAmountChanged", _component, totalAmount);

        // Assert"
        Assert.Equal($"Total: ${totalAmount:F2}", _component.GetPrivatePropertyValue<string>("PaymentTitleBDP"));
    }

    [Fact]
    public void Dispose_ShouldDetachFromTotalAmountChangedEvent()
    {
        // Arrange
        var wasEventDetached = false;

        _mockPaymentTotalService.SetupAdd(service => service.TotalAmountChanged += It.IsAny<Action<double>>());
        _mockPaymentTotalService.SetupRemove(service => service.TotalAmountChanged -= It.IsAny<Action<double>>())
                               .Callback(() => wasEventDetached = true);

        // Act
        _component.Dispose();

        // Assert
        Assert.True(wasEventDetached, "The TotalAmountChanged event handler was not properly detached upon disposal.");
    }
}