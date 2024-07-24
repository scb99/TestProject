using DBExplorerBlazor.Services;

namespace MenuItemComponents;

public class ExpiredMembershipsGetTitleServiceTests
{
    private readonly ExpiredMembershipsGetTitleService service = new();

    [Fact]
    public void GetTitle_SingleMembership_ReturnsSingularTitle()
    {
        // Arrange
        int count = 1;
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var expectedTitle = "1 expired membership between 1/1/2020 and 12/31/2020";

        // Act
        var title = service.GetTitle(count, startDate, endDate);

        // Assert
        Assert.Equal(expectedTitle, title);
    }

    [Fact]
    public void GetTitle_MultipleMemberships_ReturnsPluralTitle()
    {
        // Arrange
        int count = 2;
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var expectedTitle = "2 expired memberships between 1/1/2020 and 12/31/2020";

        // Act
        var title = service.GetTitle(count, startDate, endDate);

        // Assert
        Assert.Equal(expectedTitle, title);
    }
}