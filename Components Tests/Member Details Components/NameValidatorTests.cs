using DBExplorerBlazor.Components;

namespace MenuItemComponents;

public class NameValidatorTests
{
    [Theory]
    [InlineData("John Doe", true)]
    [InlineData("John", true)]
    [InlineData("J", true)]
    [InlineData("John Doe I", true)]
    [InlineData("John Doe IIII", false)]
    [InlineData("John123", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidName_ReturnsExpectedResult(string name, bool expected)
    {
        // Act
        var result = NameValidator.IsValidName(name);

        // Assert
        Assert.Equal(expected, result);
    }
}