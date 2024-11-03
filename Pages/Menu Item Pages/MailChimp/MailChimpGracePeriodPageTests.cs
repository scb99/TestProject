using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace MenuItemPages;

public class MailChimpGracePeriodPageTests
{
    [Fact]
    public void OnYesLogoutButtonClicked_SetsGracePeriodTo180()
    {
        // Arrange
        var page = new MailChimpGracePeriodPage();

        // Act
        page.OnYesLogoutButtonClicked();

        // Assert
        Assert.Equal(180, page.GracePeriod);
    }

    [Fact]
    public void OnNoLogoutButtonClicked_NavigatesToWelcomePage()
    {
        // Arrange
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();
        var page = new MailChimpGracePeriodPage
        {
            NavigationService = mockNavigationService.Object
        };

        // Act
        page.OnNoLogoutButtonClicked();

        // Assert
        mockNavigationService.Verify(service => service.NavigateTo("WelcomePage"), Times.Once);
    }

    [Fact]
    public void CanShowMailChimpNames_ReturnsTrue_WhenGracePeriodIsNotMinusOne()
    {
        // Arrange
        var page = new MailChimpGracePeriodPage
        {
            GracePeriod = 180
        };

        // Act
        var result = page.CanShowMailChimpNames;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowMailChimpNames_ReturnsFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new MailChimpGracePeriodPage
        {
            GracePeriod = -1
        };

        // Act
        var result = page.CanShowMailChimpNames;

        // Assert
        Assert.False(result);
    }
}