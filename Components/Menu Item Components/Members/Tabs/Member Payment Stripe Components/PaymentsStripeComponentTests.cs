using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class PaymentsStripeComponentTests
{
    private readonly PaymentsStripeComponent _component;
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;

    public PaymentsStripeComponentTests()
    {
        _mockStripeService = new Mock<ICrossCuttingStripeService>();
        _component = new PaymentsStripeComponent
        {
            StripeService = _mockStripeService.Object
        };
    }

    [Fact]
    public void OnInitialized_ShouldSubscribeToStripePaymentsChanged()
    {
        // Act
        _component.OnInitialized2();

        // Assert
        _mockStripeService.VerifyAdd(s => s.StripePaymentsChanged += It.IsAny<System.Action>(), Times.Once);
    }

    [Fact]
    public void OnInitialized2_ShouldCallOnInitialized()
    {
        // Act
        _component.OnInitialized2();

        // Assert
        _mockStripeService.VerifyAdd(s => s.StripePaymentsChanged += It.IsAny<System.Action>(), Times.Once);
    }

    [Fact]
    public void HandleRowSelectionChanged_ShouldUpdateStripeMemberID()
    {
        // Arrange
        var args = new RowSelectEventArgs<StripeEntity>
        {
            Data = new StripeEntity { ID = 1 }
        };

        // Act
        _component.HandleRowSelectionChanged(args);

        // Assert
        _mockStripeService.VerifySet(s => s.StripeMemberID = 1, Times.Once);
    }

    [Fact]
    public void Dispose_ShouldUnsubscribeFromStripePaymentsChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _mockStripeService.VerifyRemove(s => s.StripePaymentsChanged -= It.IsAny<System.Action>(), Times.Once);
    }
}