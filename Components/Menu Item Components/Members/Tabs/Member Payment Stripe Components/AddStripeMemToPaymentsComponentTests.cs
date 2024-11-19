using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class AddStripeMemToPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;
    private readonly Mock<IStripePaymentHandlerService> _mockStripePaymentHandlerService;
    private readonly AddStripeMemToPaymentsComponent _component;

    public AddStripeMemToPaymentsComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockStripeService = new Mock<ICrossCuttingStripeService>();
        _mockStripePaymentHandlerService = new Mock<IStripePaymentHandlerService>();

        _component = new AddStripeMemToPaymentsComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("StripeService", _mockStripeService.Object);
        _component.SetPrivatePropertyValue("StripePaymentHandlerService", _mockStripePaymentHandlerService.Object);
    }

    [Fact]
    public void OnInitialized_ShouldSubscribeToStripeMemberIDChanged()
    {
        // Act
        typeof(AddStripeMemToPaymentsComponent).Invoke("OnInitialized", _component);

        // Assert
        _mockStripeService.VerifyAdd(s => s.StripeMemberIDChanged += It.IsAny<Action<int>>(), Times.Once);
    }

    [Fact]
    public void OnAddButtonClicked_ShouldCallAddStripePayment()
    {
        // Arrange
        _component.SetPrivateMemberValue<int>("_selectedStripeMemberID", 1);

        // Act
        typeof(AddStripeMemToPaymentsComponent).Invoke("OnAddButtonClicked", _component);

        // Assert
        _mockStripePaymentHandlerService.Verify(s => s.AddStripePayment(1), Times.Once);
    }

    [Fact]
    public void OnStripeMemberIDChange_ShouldUpdateDisabledAndSelectedStripeMemberID()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        typeof(AddStripeMemToPaymentsComponent).Invoke("OnStripeMemberIDChange", _component, 1);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("Disabled"));
        Assert.Equal(1, _component.GetPrivateMemberValue<int>("_selectedStripeMemberID"));
    }

    [Fact]
    public void OnStripeMemberIDChange_ShouldDisableButtonWhenIDIsZero()
    {
        // Arrange
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        typeof(AddStripeMemToPaymentsComponent).Invoke("OnStripeMemberIDChange", _component, 0);

        // Assert
        Assert.True(_component.GetPrivatePropertyValue<bool>("Disabled"));
        Assert.Equal(0, _component.GetPrivateMemberValue<int>("_selectedStripeMemberID"));
    }

    [Fact]
    public void Dispose_ShouldUnsubscribeFromStripeMemberIDChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _mockStripeService.VerifyRemove(s => s.StripeMemberIDChanged -= It.IsAny<Action<int>>(), Times.Once);
    }
}