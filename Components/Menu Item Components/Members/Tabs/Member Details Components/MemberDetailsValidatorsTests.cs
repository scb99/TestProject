using DBExplorerBlazor.Components;

namespace MenuItemComponents;

public class MemberDetailsValidatorsTests
{
    [Theory]
    [InlineData("Address1", "123 Main St", true)]
    [InlineData("Billing Email", "test@example.com", true)]
    [InlineData("Billing Email", "invalid-email", false)]
    [InlineData("Billing First Name", "John", true)]
    [InlineData("Billing Last Name", "Doe", true)]
    [InlineData("Birth Date", "12/31/2000", true)]
    [InlineData("Birth Date", "31/12/2000", false)]
    [InlineData("City", "New York", true)]
    [InlineData("City", "new york", false)]
    [InlineData("Email Address", "test@example.com", true)]
    [InlineData("Email Address", "invalid-email", false)]
    [InlineData("First Name", "John", true)]
    [InlineData("Home Phone", "223-456-7890", true)]
    [InlineData("Home Phone", "123-456-7890", false)]
    [InlineData("Last Name", "Doe", true)]
    [InlineData("Other Phone", "223-456-7890", true)]
    [InlineData("Other Phone", "123-456-7890", false)]
    [InlineData("State", "CA", true)]
    [InlineData("State", "XYZ", false)]
    [InlineData("Zip", "12345", true)]
    [InlineData("Zip", "1234", false)]
    [InlineData("Unknown Field", "Any Value", true)]
    public void IsMemberDetailsADataValid_ReturnsExpectedResult(string displayName, string value, bool expected)
    {
        // Act
        var result = MemberDetailsValidators.IsMemberDetailsADataValid(displayName, value);

        // Assert
        Assert.Equal(expected, result);
    }
}