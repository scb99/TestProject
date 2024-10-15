using DBExplorerBlazor.Components;

namespace MenuItemComponents;

public class StateNameValidatorTests
{
    [Theory]
    [InlineData("CA", true)]
    [InlineData("NY", true)]
    [InlineData("XYZ", false)]
    [InlineData("C", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsLegalAbbreviation_ReturnsExpectedResult(string stateName, bool expected)
    {
        // Act
        var result = StateNameValidator.IsLegalAbbreviation(stateName);

        // Assert
        Assert.Equal(expected, result);
    }
}