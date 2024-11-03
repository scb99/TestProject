using DBExplorerBlazor.Pages;

namespace MenuItemPages;

public class CompleteRosterGracePeriodPageTests
{
    [Fact]
    public void CanShowRoster_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new CompleteRosterGracePeriodPage();

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowRoster_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new CompleteRosterGracePeriodPage
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
        var page = new CompleteRosterGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.True(result);
    }
}