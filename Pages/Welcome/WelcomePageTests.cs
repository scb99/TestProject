using DBExplorerBlazor.Pages;

namespace Pages;

public class WelcomePageTests
{
    [Fact]
    public void WelcomePage_CanBeInstantiated()
    {
        // Arrange & Act
        var page = new WelcomePage();

        // Assert
        Assert.NotNull(page);
    }
}