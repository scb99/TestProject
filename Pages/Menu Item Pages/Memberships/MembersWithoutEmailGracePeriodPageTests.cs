using DBExplorerBlazor.Pages;

namespace MenuItemPages;

public class MembersWithoutEmailGracePeriodPageTests
{
    [Fact]
    public void CanShowListOfMembersWithoutEmailAddresses_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new MembersWithoutEmailGracePeriodPage();

        // Act
        var result = page.CanShowListOfMembersWithoutEmailAddresses;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowListOfMembersWithoutEmailAddresses_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new MembersWithoutEmailGracePeriodPage
        {
            GracePeriod = 10
        };

        // Act
        var result = page.CanShowListOfMembersWithoutEmailAddresses;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowListOfMembersWithoutEmailAddresses_ShouldReturnTrue_WhenGracePeriodIsZero()
    {
        // Arrange
        var page = new MembersWithoutEmailGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowListOfMembersWithoutEmailAddresses;

        // Assert
        Assert.True(result);
    }
}