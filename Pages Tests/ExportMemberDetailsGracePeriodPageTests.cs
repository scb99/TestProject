using DBExplorerBlazor.Pages;

namespace Pages;

public class ExportMemberDetailsGracePeriodPageTests
{
    [Fact]
    public void CanShowListOfMemberDetails_ShouldReturnFalse_WhenGracePeriodIsMinusOne()
    {
        // Arrange
        var page = new ExportMemberDetailsGracePeriodPage();

        // Act
        var result = page.CanShowListOfMemberDetails;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowListOfMemberDetails_ShouldReturnTrue_WhenGracePeriodIsSet()
    {
        // Arrange
        var page = new ExportMemberDetailsGracePeriodPage
        {
            GracePeriod = 10
        };

        // Act
        var result = page.CanShowListOfMemberDetails;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowListOfMemberDetails_ShouldReturnTrue_WhenGracePeriodIsZero()
    {
        // Arrange
        var page = new ExportMemberDetailsGracePeriodPage
        {
            GracePeriod = 0
        };

        // Act
        var result = page.CanShowListOfMemberDetails;

        // Assert
        Assert.True(result);
    }
}