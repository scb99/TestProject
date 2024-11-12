using DBExplorerBlazor.Pages;

namespace MenuItemPages;

public class TotalMembersChartPageTests
{
    [Fact]
    public void TotalMembersChartPage_CanBeInstantiated()
    {
        // Arrange & Act
        var page = new TotalMembersChartPage();

        // Assert
        Assert.NotNull(page);
    }
}