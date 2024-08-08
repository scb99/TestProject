using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Authorization;

public class AuthorizationServiceTests
{
    [Theory]
    [InlineData("SuperUser", true)]
    [InlineData("Admin", true)]
    [InlineData("RegularUser", false)]
    [InlineData("Guest", false)]
    public void IsAuthorized_ReturnsExpectedResult(string role, bool expectedResult)
    {
        // Arrange
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns(role);

        var authorizationService = new AuthorizationService(mockLoggedInMemberService.Object);

        // Act
        var result = authorizationService.IsAuthorized();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}