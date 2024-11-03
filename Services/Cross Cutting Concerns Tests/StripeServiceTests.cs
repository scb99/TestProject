using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class StripeServiceTests
{
    [Fact]
    public void StripeEntities_Setter_InvokesStripePaymentsChangedEvent()
    {
        // Arrange
        var stripeService = new StripeService();
        var eventInvoked = false;
        stripeService.StripePaymentsChanged += () => eventInvoked = true;

        // Act
        stripeService.StripeEntities = new List<StripeEntity>();

        // Assert
        Assert.True(eventInvoked);
    }

    [Fact]
    public void StripeMemberID_Setter_InvokesStripeMemberIDChangedEvent()
    {
        // Arrange
        var stripeService = new StripeService();
        var eventInvoked = false;
        stripeService.StripeMemberIDChanged += (id) => eventInvoked = true;

        // Act
        stripeService.StripeMemberID = 123;

        // Assert
        Assert.True(eventInvoked);
    }

    [Fact]
    public void StripeEntities_Getter_ReturnsCorrectValue()
    {
        // Arrange
        var stripeService = new StripeService();
        var expectedEntities = new List<StripeEntity> { new() };
        stripeService.StripeEntities = expectedEntities;

        // Act
        var actualEntities = stripeService.StripeEntities;

        // Assert
        Assert.Equal(expectedEntities, actualEntities);
    }

    [Fact]
    public void StripeMemberID_Getter_ReturnsCorrectValue()
    {
        // Arrange
        var stripeService = new StripeService();
        var expectedID = 123;
        stripeService.StripeMemberID = expectedID;

        // Act
        var actualID = stripeService.StripeMemberID;

        // Assert
        Assert.Equal(expectedID, actualID);
    }
}