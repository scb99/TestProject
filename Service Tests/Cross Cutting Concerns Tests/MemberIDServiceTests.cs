using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class MemberIDServiceTests
{
    [Fact]
    public void MemberID_GetSet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new MemberIDService();
        int expected = 5;

        // Act
        service.MemberID = expected;
        var actual = service.MemberID;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MemberID_SetNonZeroValue_TriggersOnChange()
    {
        // Arrange
        var service = new MemberIDService();
        bool eventFired = false;
        service.MemberIDOnChange += () => eventFired = true;

        // Act
        service.MemberID = 1;

        // Assert
        Assert.True(eventFired);
    }

    [Fact]
    public void MemberID_SetZeroValue_DoesNotTriggerOnChange()
    {
        // Arrange
        var service = new MemberIDService();
        bool eventFired = false;
        service.MemberIDOnChange += () => eventFired = true;

        // Act
        service.MemberID = 0; // Initial value is also 0, so setting to 0 again should not trigger the event.

        // Assert
        Assert.False(eventFired);
    }
}