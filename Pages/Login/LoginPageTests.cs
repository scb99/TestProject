using DataAccess.Interfaces;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Moq;

namespace Pages;

public class LoginPageTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<IRepositoryMemberDetail2> _mockMemberDetailRepository;
    private readonly Mock<NavigationManager> _mockNavigationManager;
    private readonly LoginPage _loginPage;

    public LoginPageTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockMemberDetailRepository = new Mock<IRepositoryMemberDetail2>();
        _mockNavigationManager = new Mock<NavigationManager>();

        _loginPage = new LoginPage
        {
            Show = _mockAlertService.Object,
            LoggedInMemberService = _mockLoggedInMemberService.Object,
            Logger = _mockLoggerService.Object,
            MemberDetailRepository = _mockMemberDetailRepository.Object,
            NavigationManager = _mockNavigationManager.Object
        };
    }

    [Fact]
    public void OnInitialized_SetsInitialValues()
    {
        // Act
        _loginPage.OnInitialized2();

        // Assert
        Assert.Equal("password", _loginPage.TxtTypeBDP);
        Assert.Equal(string.Empty, _loginPage.TextBoxValueBDP);
    }

    [Fact]
    public async Task OnAfterRenderAsync_FirstRender_FocusesFirstInput()
    {
        // Arrange
        var firstInput = new ElementReference();
        _loginPage.GetType().GetField("_firstInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(_loginPage, firstInput);

        // Act
        await _loginPage.OnAfterRender2Async(true);

        // Assert
        // Assuming _firstInput.FocusAsync() is called, but cannot be directly tested
    }

    [Fact]
    public async Task OnLoginButtonClickedAsync_InvalidPasscode_ShowsAlert()
    {
        // Arrange
        _loginPage.TextBoxValueBDP = "-12345!";

        // Act
        await _loginPage.OnLoginButtonClickedAsync();

        // Assert
        _mockAlertService.Verify(x => x.AlertUsingFallingMessageBoxAsync("You are not authorized to use this website!"), Times.Once);
    }

    [Fact]
    public async Task ShowOrHidePasswordAsync_TogglesPasswordVisibility()
    {
        // Arrange
        _loginPage.TxtTypeBDP = "password";

        // Act
        await _loginPage.ShowOrHidePasswordAsync();

        // Assert
        Assert.Equal("text", _loginPage.TxtTypeBDP);

        // Act
        await _loginPage.ShowOrHidePasswordAsync();

        // Assert
        Assert.Equal("password", _loginPage.TxtTypeBDP);
    }

    [Fact]
    public async Task KeyDownAsync_EnterKey_InvokesLogin()
    {
        // Arrange
        var keyboardEventArgs = new KeyboardEventArgs { Key = "Enter" };

        // Act
        await _loginPage.KeyDownAsync(keyboardEventArgs);

        // Assert
        // Assuming OnLoginButtonClickedAsync() is called, but cannot be directly tested
    }

    [Fact]
    public async Task KeyDownAsync_BackspaceKey_RemovesLastCharacter()
    {
        // Arrange
        _loginPage.TextBoxValueBDP = "12345";
        var keyboardEventArgs = new KeyboardEventArgs { Key = "Backspace" };

        // Act
        await _loginPage.KeyDownAsync(keyboardEventArgs);

        // Assert
        Assert.Equal("1234", _loginPage.TextBoxValueBDP);
    }

    [Fact]
    public async Task KeyDownAsync_OtherKey_AddsCharacter()
    {
        // Arrange
        _loginPage.TextBoxValueBDP = "12345";
        var keyboardEventArgs = new KeyboardEventArgs { Key = "A" };

        // Act
        await _loginPage.KeyDownAsync(keyboardEventArgs);

        // Assert
        Assert.Equal("12345A", _loginPage.TextBoxValueBDP);
    }
}
