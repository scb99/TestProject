using DBExplorerBlazor.Pages;

namespace Pages;

public class ExpiredMembershipsDatePickerPageTests
{
    [Fact]
    public void CanShowListOfExpiredMemberships_ShouldReturnFalse_WhenDatesAreMinValue()
    {
        // Arrange
        var page = new ExpiredMembershipsDatePickerPage();

        // Act
        var result = page.CanShowListOfExpiredMemberships;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowListOfExpiredMemberships_ShouldReturnTrue_WhenDatesAreSet()
    {
        // Arrange
        var page = new ExpiredMembershipsDatePickerPage
        {
            StartDateBDP = new DateTime(2023, 1, 1),
            EndDateBDP = new DateTime(2023, 12, 31)
        };

        // Act
        var result = page.CanShowListOfExpiredMemberships;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowListOfExpiredMemberships_ShouldReturnFalse_WhenOnlyStartDateIsSet()
    {
        // Arrange
        var page = new ExpiredMembershipsDatePickerPage
        {
            StartDateBDP = new DateTime(2023, 1, 1)
        };

        // Act
        var result = page.CanShowListOfExpiredMemberships;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowListOfExpiredMemberships_ShouldReturnFalse_WhenOnlyEndDateIsSet()
    {
        // Arrange
        var page = new ExpiredMembershipsDatePickerPage
        {
            EndDateBDP = new DateTime(2023, 12, 31)
        };

        // Act
        var result = page.CanShowListOfExpiredMemberships;

        // Assert
        Assert.False(result);
    }
}