using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class LoggedInMemberServiceTests
{
    [Fact]
    public void MemberGreeting_GetSet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new LoggedInMemberService();
        var expectedGreeting = "Hello, User!";

        // Act
        service.MemberGreeting = expectedGreeting;
        var actualGreeting = service.MemberGreeting;

        // Assert
        Assert.Equal(expectedGreeting, actualGreeting);
    }

    [Fact]
    public void MemberRole_GetSet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new LoggedInMemberService();
        var expectedRole = "Administrator";

        // Act
        service.MemberRole = expectedRole;
        var actualRole = service.MemberRole;

        // Assert
        Assert.Equal(expectedRole, actualRole);
    }

    [Fact]
    public void MemberUserID_GetSet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new LoggedInMemberService();
        var expectedUserID = 123;

        // Act
        service.MemberUserID = expectedUserID;
        var actualUserID = service.MemberUserID;

        // Assert
        Assert.Equal(expectedUserID, actualUserID);
    }
}