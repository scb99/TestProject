using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class AddStripeMemToPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;
    private readonly Mock<IStripePaymentHandlerService> _mockStripePaymentHandlerService;
    private readonly AddStripeMemToPaymentsComponent _component;

    public AddStripeMemToPaymentsComponentTests()
    {
        _mockStripeService = new Mock<ICrossCuttingStripeService>();
        _mockStripePaymentHandlerService = new Mock<IStripePaymentHandlerService>();

        _component = new AddStripeMemToPaymentsComponent
        {
            StripeService = _mockStripeService.Object,
            StripePaymentHandlerService = _mockStripePaymentHandlerService.Object
        };
    }

    [Fact]
    public void OnInitialized_ShouldSubscribeToStripeMemberIDChanged()
    {
        // Act
        _component.OnInitialized2();

        // Assert
        _mockStripeService.VerifyAdd(s => s.StripeMemberIDChanged += It.IsAny<Action<int>>(), Times.Once);
    }

    [Fact]
    public void OnAddButtonClicked_ShouldCallAddStripePayment()
    {
        // Arrange
        _component._selectedStripeMemberID = 1;

        // Act
        _component.OnAddButtonClicked();

        // Assert
        _mockStripePaymentHandlerService.Verify(s => s.AddStripePayment(1), Times.Once);
    }

    //[Fact]
    //public void OnStripeMemberIDChange_ShouldUpdateDisabledAndSelectedStripeMemberID()
    //{
    //    // Act
    //    _component.OnStripeMemberIDChange(1);

    //    // Assert
    //    Assert.False(_component.Disabled);
    //    Assert.Equal(1, _component._selectedStripeMemberID);
    //}

    //[Fact]
    //public void OnStripeMemberIDChange_ShouldDisableButtonWhenIDIsZero()
    //{
    //    // Act
    //    _component.OnStripeMemberIDChange(0);

    //    // Assert
    //    Assert.True(_component.Disabled);
    //    Assert.Equal(0, _component._selectedStripeMemberID);
    //}

    [Fact]
    public void Dispose_ShouldUnsubscribeFromStripeMemberIDChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _mockStripeService.VerifyRemove(s => s.StripeMemberIDChanged -= It.IsAny<Action<int>>(), Times.Once);
    }
}