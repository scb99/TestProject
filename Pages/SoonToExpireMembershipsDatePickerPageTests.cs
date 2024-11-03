using DBExplorerBlazor.Pages;

namespace Pages;

public class SoonToExpireMembershipsDatePickerPageTests
{
    [Fact]
    public void CanShowSoonToExpireMemberships_ShouldReturnFalse_WhenEndDateIsMinValue()
    {
        // Arrange
        var page = new SoonToExpireMembershipsDatePickerPage();

        // Act
        var result = page.CanShowSoonToExpireMemberships;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowSoonToExpireMemberships_ShouldReturnTrue_WhenEndDateIsSet()
    {
        // Arrange
        var page = new SoonToExpireMembershipsDatePickerPage
        {
            EndDate = new DateTime(2023, 12, 31)
        };

        // Act
        var result = page.CanShowSoonToExpireMemberships;

        // Assert
        Assert.True(result);
    }
}