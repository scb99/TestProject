using DBExplorerBlazor.Pages;

namespace Pages;

public class RosterRecipientsGracePeriodPageTests
{
    [Fact]
    public void CanShowRosterRecipientsComponent_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new RosterRecipientsGracePeriodPage();

        // Act
        var result = page.CanShowRosterRecipientsComponent;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowRosterRecipientsComponent_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new RosterRecipientsGracePeriodPage
        {
            GracePeriod = 10
        };

        // Act
        var result = page.CanShowRosterRecipientsComponent;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowRosterRecipientsComponent_ShouldReturnTrue_WhenGracePeriodIsZero()
    {
        // Arrange
        var page = new RosterRecipientsGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowRosterRecipientsComponent;

        // Assert
        Assert.True(result);
    }
}