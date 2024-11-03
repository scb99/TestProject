using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MainLayout;

public class MainLayoutServiceTests
{
    private readonly Mock<ICrossCuttingLoggedInMemberService> mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoggerService> mockLogger;
    private readonly MainLayoutService mainLayoutService;

    public MainLayoutServiceTests()
    {
        mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        mockLogger = new Mock<ICrossCuttingLoggerService>();
        mainLayoutService = new MainLayoutService(mockLoggedInMemberService.Object, mockLogger.Object);
    }

    [Theory]
    [InlineData("About", "Selected About Menu Item")]
    [InlineData("Catch All", "Selected Catch All Menu Item")]
    [InlineData("Members", "Selected Members Menu Item")]
    [InlineData("Treasurer", "Selected Treasurer Menu Item")]
    [InlineData("Process Vanguard csv File...", "Selected Process Vanguard csv File.. Submenu Item ")]
    public async Task HandleMenuItemSelectedAsync_LogsCorrectMessage(string menuItem, string expectedLogMessage)
    {
        // Act
        await mainLayoutService.HandleMenuItemSelectedAsync(menuItem);

        // Assert
        mockLogger.Verify(logger => logger.LogResultAsync(expectedLogMessage), Times.Once);
    }

    [Fact]
    public void GetMemberGreeting_ReturnsCorrectGreeting()
    {
        // Arrange
        var expectedGreeting = "Hello, Member!";
        mockLoggedInMemberService.Setup(service => service.MemberGreeting).Returns(expectedGreeting);

        // Act
        var result = mainLayoutService.GetMemberGreeting();

        // Assert
        Assert.Equal(expectedGreeting, result);
    }

    [Fact]
    public void IsAuthorizedToSeeEliminateDuplicateAccountsMenuItem_ReturnsTrueForSuperUser()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeEliminateDuplicateAccountsMenuItem();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAuthorizedToSeeEliminateDuplicateAccountsMenuItem_ReturnsFalseForNonSuperUser()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("User");

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeEliminateDuplicateAccountsMenuItem();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsAuthorizedToSeeExtrasMenuItem_ReturnsTrueForSpecificUserID()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberUserID).Returns(3386);

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeExtrasMenuItem();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAuthorizedToSeeExtrasMenuItem_ReturnsFalseForDifferentUserID()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberUserID).Returns(1234);

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeExtrasMenuItem();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsAuthorizedToSeeLogs_ReturnsTrueForSuperUser()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeLogs();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAuthorizedToSeeLogs_ReturnsFalseForNonSuperUser()
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("User");

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeLogs();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("SuperUser", true)]
    [InlineData("Admin", true)]
    [InlineData("User", false)]
    public void IsAuthorizedToSeeMenuItem_ReturnsCorrectAuthorization(string role, bool expectedResult)
    {
        // Arrange
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns(role);

        // Act
        var result = mainLayoutService.IsAuthorizedToSeeMenuItem();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}