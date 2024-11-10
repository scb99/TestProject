using DBExplorerBlazor;

namespace ExtensionMethods;

public class IntExtensionsTests
{
    [Fact]
    public void SingularOrPlural_ReturnsEmptyString_WhenCountIsOne()
    {
        // Arrange
        int count = 1;

        // Act
        var result = count.SingularOrPlural();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SingularOrPlural_ReturnsS_WhenCountIsZero()
    {
        // Arrange
        int count = 0;

        // Act
        var result = count.SingularOrPlural();

        // Assert
        Assert.Equal("s", result);
    }

    [Fact]
    public void SingularOrPlural_ReturnsS_WhenCountIsGreaterThanOne()
    {
        // Arrange
        int count = 5;

        // Act
        var result = count.SingularOrPlural();

        // Assert
        Assert.Equal("s", result);
    }

    [Fact]
    public void SingularOrPlural_ReturnsS_WhenCountIsNegative()
    {
        // Arrange
        int count = -1;

        // Act
        var result = count.SingularOrPlural();

        // Assert
        Assert.Equal("s", result);
    }
}