using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace MenuItemComponents;

public class LogoutPageTests
{
    private readonly Mock<ILogoutService> _mockLogoutService;
    private readonly LogoutPage _logoutPage;

    public LogoutPageTests()
    {
        _mockLogoutService = new Mock<ILogoutService>();
        _logoutPage = new LogoutPage
        {
            LogoutService = _mockLogoutService.Object
        };
    }

    [Fact]
    public async Task OnYesLogoutButtonClickedAsync_CallsHandleLogoutAsync()
    {
        // Act
        await _logoutPage.OnYesLogoutButtonClickedAsync();

        // Assert
        _mockLogoutService.Verify(service => service.HandleLogoutAsync(), Times.Once);
    }

    [Fact]
    public async Task OnNoLogoutButtonClickedAsync_CallsHandleCancelLogoutAsync()
    {
        // Act
        await _logoutPage.OnNoLogoutButtonClickedAsync();

        // Assert
        _mockLogoutService.Verify(service => service.HandleCancelLogoutAsync(), Times.Once);
    }
}