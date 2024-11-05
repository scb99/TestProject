using DBExplorerBlazor;

namespace ExtensionMethods;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ConvertToStringDate_ReturnsCorrectFormat()
    {
        // Arrange
        var dateTime = new DateTime(2023, 10, 5);

        // Act
        var result = dateTime.ConvertToStringDate();

        // Assert
        Assert.Equal("2023-10-05", result);
    }

    [Fact]
    public void ConvertToStringDate_HandlesSingleDigitMonthAndDay()
    {
        // Arrange
        var dateTime = new DateTime(2023, 1, 9);

        // Act
        var result = dateTime.ConvertToStringDate();

        // Assert
        Assert.Equal("2023-01-09", result);
    }

    [Fact]
    public void ConvertToStringDate_HandlesLeapYear()
    {
        // Arrange
        var dateTime = new DateTime(2020, 2, 29);

        // Act
        var result = dateTime.ConvertToStringDate();

        // Assert
        Assert.Equal("2020-02-29", result);
    }

    [Fact]
    public void ConvertToStringDate_HandlesEndOfYear()
    {
        // Arrange
        var dateTime = new DateTime(2023, 12, 31);

        // Act
        var result = dateTime.ConvertToStringDate();

        // Assert
        Assert.Equal("2023-12-31", result);
    }
}