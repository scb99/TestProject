using DBExplorerBlazor.Pages;

namespace MenuItemComponents;

public class AboutPageTests
{
    [Fact]
    public void AboutPage_SetsYearBDPToCurrentYear()
    {
        // Arrange
        var currentYear = DateTime.Now.Year.ToString();
        var aboutPage = new AboutPage();

        // Act
        var yearBDPField = aboutPage.GetType().GetProperty("YearBDP", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var yearBDPValue = yearBDPField?.GetValue(aboutPage)?.ToString();

        // Assert
        Assert.Equal(currentYear, yearBDPValue);
    }
}