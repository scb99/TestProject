using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace MenuItemPages;

public class MailChimpMembersPageTests
{
    [Fact]
    public void OnYesLogoutButtonClicked_SetsCanShowMailChimpNamesToTrue()
    {
        // Arrange
        var page = new MailChimpMembersPage();

        // Act
        page.OnYesLogoutButtonClicked();

        // Assert
        Assert.True(page.CanShowMailChimpNames);
    }

    [Fact]
    public void OnNoLogoutButtonClicked_NavigatesToWelcomePage()
    {
        // Arrange
        var mockNavigationService = new Mock<ICrossCuttingNavigationService>();
        var page = new MailChimpMembersPage
        {
            NavigationService = mockNavigationService.Object
        };

        // Act
        page.OnNoLogoutButtonClicked();

        // Assert
        mockNavigationService.Verify(service => service.NavigateTo("WelcomePage"), Times.Once);
    }
}