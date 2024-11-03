using DBExplorerBlazor.Services;

namespace ExpiredMemberships;

public class ExpiredMembershipsGetTitleServiceTests
{
    [Fact]
    public void GetTitle_SingularCount_ReturnsCorrectTitle()
    {
        // Arrange
        var service = new ExpiredMembershipsGetTitleService();
        var count = 1;
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        var result = service.GetTitle(count, startDate, endDate);

        // Assert
        Assert.Equal("1 expired membership between 1/1/2023 and 12/31/2023", result);
    }

    [Fact]
    public void GetTitle_PluralCount_ReturnsCorrectTitle()
    {
        // Arrange
        var service = new ExpiredMembershipsGetTitleService();
        var count = 5;
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        var result = service.GetTitle(count, startDate, endDate);

        // Assert
        Assert.Equal("5 expired memberships between 1/1/2023 and 12/31/2023", result);
    }
}