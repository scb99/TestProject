using DBExplorerBlazor;

namespace ExtensionMethods;

public class StringExtensionsTests
{
    [Fact]
    public void ConvertMemberStatus_ReturnsCorrectStatus_ForValidDays()
    {
        // Arrange
        var inputDays = "45";

        // Act
        var result = inputDays.ConvertMemberStatus();

        // Assert
        Assert.Equal("31-60 days", result);
    }

    [Fact]
    public void ConvertMemberStatus_ReturnsCorrectStatus_ForInvalidDays()
    {
        // Arrange
        var inputDays = "invalid";

        // Act
        var result = inputDays.ConvertMemberStatus();

        // Assert
        Assert.Equal("Member in Good Standing", result);
    }

    [Fact]
    public void ConvertMemberStatus_ReturnsCorrectStatus_ForNegativeDays()
    {
        // Arrange
        var inputDays = "-10";

        // Act
        var result = inputDays.ConvertMemberStatus();

        // Assert
        Assert.Equal("Member in Good Standing", result);
    }

    [Fact]
    public void ConvertMemberStatus_ReturnsCorrectStatus_ForZeroDays()
    {
        // Arrange
        var inputDays = "0";

        // Act
        var result = inputDays.ConvertMemberStatus();

        // Assert
        Assert.Equal("0-30 days", result);
    }

    [Fact]
    public void ConvertMemberStatus_ReturnsCorrectStatus_ForMoreThan120Days()
    {
        // Arrange
        var inputDays = "150";

        // Act
        var result = inputDays.ConvertMemberStatus();

        // Assert
        Assert.Equal(">120 days", result);
    }
}