using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
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

        _component = new AddStripeMemToPaymentsComponent
        {
            Execute = _mockExecute.Object,
            StripeService = _mockStripeService.Object,
            StripePaymentHandlerService = _mockStripePaymentHandlerService.Object
        };
    }

    [Fact]
    public void OnInitialized_SubscribesToStripeMemberIDChanged()
    {
        // Act
        _component.OnInitialized2();

        // Assert
        _mockStripeService.VerifyAdd(service => service.StripeMemberIDChanged += It.IsAny<Action<int>>(), Times.Once);
    }

    [Fact]
    public void OnAddButtonClicked_CallsAddStripePayment()
    {
        // Arrange
        _component._selectedStripeMemberID = 123;

        // Act
        _component.OnAddButtonClicked();

        // Assert
        _mockStripePaymentHandlerService.Verify(service => service.AddStripePayment(123), Times.Once);
    }

    [Fact]
    public void OnStripeMemberIDChange_UpdatesDisabledAndSelectedStripeMemberID()
    {
        // Arrange
        _mockExecute.Setup(service => service.ConditionalCode()).Returns(false);

        // Act
        _component.OnStripeMemberIDChange(123);

        // Assert
        Assert.False(_component.Disabled);
        Assert.Equal(123, _component._selectedStripeMemberID);
    }

    [Fact]
    public void OnStripeMemberIDChange_DisablesButtonWhenIDIsZero()
    {
        // Arrange
        _mockExecute.Setup(service => service.ConditionalCode()).Returns(false);

        // Act
        _component.OnStripeMemberIDChange(0);

        // Assert
        Assert.True(_component.Disabled);
        Assert.Equal(0, _component._selectedStripeMemberID);
    }

    [Fact]
    public void Dispose_UnsubscribesFromStripeMemberIDChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _mockStripeService.VerifyRemove(service => service.StripeMemberIDChanged -= It.IsAny<Action<int>>(), Times.Once);
    }
}