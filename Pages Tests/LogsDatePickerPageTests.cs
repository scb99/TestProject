using DBExplorerBlazor.Pages;

namespace Pages;

public class LogsDatePickerPageTests
{
    [Fact]
    public void CanShowLogs_WhenBothDatesAreMinValue_ReturnsFalse()
    {
        // Arrange
        var page = new LogsDatePickerPage
        {
            EndDateBDP = DateTime.MinValue,
            StartDateBDP = DateTime.MinValue
        };

        // Act
        var result = page.CanShowLogs;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanShowLogs_WhenEndDateIsNotMinValue_ReturnsTrue()
    {
        // Arrange
        var page = new LogsDatePickerPage
        {
            EndDateBDP = DateTime.Now,
            StartDateBDP = DateTime.MinValue
        };

        // Act
        var result = page.CanShowLogs;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowLogs_WhenStartDateIsNotMinValue_ReturnsTrue()
    {
        // Arrange
        var page = new LogsDatePickerPage
        {
            EndDateBDP = DateTime.MinValue,
            StartDateBDP = DateTime.Now
        };

        // Act
        var result = page.CanShowLogs;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanShowLogs_WhenBothDatesAreNotMinValue_ReturnsTrue()
    {
        // Arrange
        var page = new LogsDatePickerPage
        {
            EndDateBDP = DateTime.Now,
            StartDateBDP = DateTime.Now
        };

        // Act
        var result = page.CanShowLogs;

        // Assert
        Assert.True(result);
    }
}