using DBExplorerBlazor.Pages;

namespace Pages;

public class NewsletterRecipientsGracePeriodPageTests
{
    [Fact]
    public void CanShowNewsletterRecipients_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new NewsletterRecipientsGracePeriodPage();

        // Act
        var result = page.CanShowNewsletterRecipients;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowNewsletterRecipients_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new NewsletterRecipientsGracePeriodPage
        {
            GracePeriod = 10
        };

        // Act
        var result = page.CanShowNewsletterRecipients;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowNewsletterRecipients_ShouldReturnTrue_WhenGracePeriodIsZero()
    {
        // Arrange
        var page = new NewsletterRecipientsGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowNewsletterRecipients;

        // Assert
        Assert.True(result);
    }
}