using DBExplorerBlazor.Pages;
using Microsoft.AspNetCore.Components;
using Moq;

namespace Pages;

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

    //[Fact]
    //public void OnNoLogoutButtonClicked_NavigatesToWelcomePage()
    //{
    //    // Arrange
    //    var mockNavigationManager = new Mock<NavigationManager>();
    //    var page = new MailChimpMembersPage
    //    {
    //        NavigationManager = mockNavigationManager.Object
    //    };

    //    // Act
    //    page.OnNoLogoutButtonClicked();

    //    // Assert
    //    mockNavigationManager.Verify(nm => nm.NavigateTo("WelcomePage", It.IsAny<bool>()), Times.Once);
    //}
}