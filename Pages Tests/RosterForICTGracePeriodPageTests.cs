using DBExplorerBlazor.Pages;

namespace Pages;

public class RosterForICTGracePeriodPageTests
{
    [Fact]
    public void CanShowRoster_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new RosterForICTGracePeriodPage();

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowRoster_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new RosterForICTGracePeriodPage
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
        var page = new RosterForICTGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowRoster;

        // Assert
        Assert.True(result);
    }
}