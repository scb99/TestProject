using DBExplorerBlazor.Pages;

namespace MenuItemPages;

public class RosterForPopupTennisGracePeriodPageTests
{
    [Fact]
    public void CanShowRoster_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new RosterForPopupTennisGracePeriodPage();

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowRoster_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new RosterForPopupTennisGracePeriodPage
        {
            GracePeriod = 10
        };

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowRoster_ShouldReturnTrue_WhenGracePeriodIsZero()
    {
        // Arrange
        var page = new RosterForPopupTennisGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.True(result);
    }
}