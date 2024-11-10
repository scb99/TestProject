using System.Text;
using DBExplorerBlazor;

namespace ExtensionMethods;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendSequence_AppendsItemsCorrectly()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var sequence = new List<int> { 1, 2, 3 };
        static StringBuilder appender(StringBuilder sb, int item) => sb.Append(item).Append(", ");

        // Act
        stringBuilder.AppendSequence(sequence, appender);

        // Assert
        Assert.Equal("1, 2, 3, ", stringBuilder.ToString());
    }

    [Fact]
    public void ToList_ConvertsStringBuilderContentToList()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Line 1");
        stringBuilder.AppendLine("Line 2");
        stringBuilder.AppendLine("Line 3");

        // Act
        var result = stringBuilder.ToList();

        // Assert
        var expected = new List<string> { "Line 1", "Line 2", "Line 3", "" };
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToList_HandlesEmptyStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        var result = stringBuilder.ToList();

        // Assert
        var expected = new List<string> { "" };
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AppendSequence_WithEmptySequence_DoesNotChangeStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("Initial");
        var sequence = new List<int>();
        static StringBuilder appender(StringBuilder sb, int item) => sb.Append(item).Append(", ");

        // Act
        stringBuilder.AppendSequence(sequence, appender);

        // Assert
        Assert.Equal("Initial", stringBuilder.ToString());
    }
}
