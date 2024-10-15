using DBExplorerBlazor.Components;

namespace MenuItemComponents;

public class PhoneValidatorTests
{
    [Theory]
    [InlineData("123-456-7890", false)]
    [InlineData("223-456-7890", true)]
    [InlineData("223-456-789", false)]
    [InlineData("223-456-78900", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPhoneNumber_ReturnsExpectedResult(string phoneNumber, bool expected)
    {
        // Act
        var result = PhoneValidator.IsValidPhoneNumber(phoneNumber);

        // Assert
        Assert.Equal(expected, result);
    }
}