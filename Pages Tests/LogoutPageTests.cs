using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Pages;

public class LogoutPageTests
{
    [Fact]
    public async Task OnYesLogoutButtonClickedAsync_CallsHandleLogoutAsync()
    {
        // Arrange
        var mockLogoutService = new Mock<ILogoutService>();
        var logoutPage = new LogoutPage
        {
            LogoutService = mockLogoutService.Object
        };

        // Act
        await logoutPage.OnYesLogoutButtonClickedAsync();

        // Assert
        mockLogoutService.Verify(service => service.HandleLogoutAsync(), Times.Once);
    }

    [Fact]
    public async Task OnNoLogoutButtonClickedAsync_CallsHandleCancelLogoutAsync()
    {
        // Arrange
        var mockLogoutService = new Mock<ILogoutService>();
        var logoutPage = new LogoutPage
        {
            LogoutService = mockLogoutService.Object
        };

        // Act
        await logoutPage.OnNoLogoutButtonClickedAsync();

        // Assert
        mockLogoutService.Verify(service => service.HandleCancelLogoutAsync(), Times.Once);
    }
}