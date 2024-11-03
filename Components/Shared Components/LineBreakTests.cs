using DBExplorerBlazor.Components;

namespace SharedComponents;

public class LineBreakTests
{
    [Fact]
    public void LineBreak_DefaultNum_IsOne()
    {
        // Arrange
        var lineBreak = new LineBreak();

        // Act
        var num = lineBreak.Num;

        // Assert
        Assert.Equal(1, num);
    }

    [Fact]
    public void LineBreak_OnParametersSet_SetsNumbersCorrectly()
    {
        // Arrange
        var lineBreak = new LineBreak();
        lineBreak.Initialize(5);

        // Act
        lineBreak.OnParametersSet2();
        var numbersField = lineBreak.GetType().GetField("_numbers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var numbersValue = numbersField?.GetValue(lineBreak) as List<int>;

        // Assert
        Assert.NotNull(numbersValue);
        Assert.Equal(5, numbersValue.Count);
        Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, numbersValue);
    }

    [Fact]
    public void LineBreak_OnParametersSet2_CallsOnParametersSet()
    {
        // Arrange
        var lineBreak = new LineBreak();
        lineBreak.Initialize(3);

        // Act
        lineBreak.OnParametersSet2();
        var numbersField = lineBreak.GetType().GetField("_numbers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var numbersValue = numbersField?.GetValue(lineBreak) as List<int>;

        // Assert
        Assert.NotNull(numbersValue);
        Assert.Equal(3, numbersValue.Count);
        Assert.Equal(new List<int> { 1, 2, 3 }, numbersValue);
    }
}