using DBExplorerBlazor.Pages;

namespace MenuItemPages;

public class TotalAssetsChartPageTests
{
    [Fact]
    public void TotalAssetsChartPage_CanBeInstantiated()
    {
        // Arrange & Act
        var page = new TotalAssetsChartPage();

        // Assert
        Assert.NotNull(page);
    }
}