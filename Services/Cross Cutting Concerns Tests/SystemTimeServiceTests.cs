using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class SystemTimeServiceTests
{
    [Fact]
    public void MinValue_ReturnsDateTimeMinValue()
    {
        // Arrange
        var systemTimeService = new SystemTimeService();

        // Act
        var minValue = systemTimeService.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, minValue);
    }
}