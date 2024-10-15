using DBExplorerBlazor.Components;

namespace MenuItemComponents;

public class CommonValidatorTests
{
    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("null", false)]
    public void IsNotEmptyString_ReturnsExpectedResult(string value, bool expected)
    {
        // Act
        var result = CommonValidator.IsNotEmptyString(value);

        // Assert
        Assert.Equal(expected, result);
    }
}