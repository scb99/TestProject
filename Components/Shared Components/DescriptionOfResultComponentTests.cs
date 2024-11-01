using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DescriptionOfResultComponentTests
{
    [Fact]
    public void Title_ShouldBeEmptyStringByDefault()
    {
        // Arrange
        var component = new DescriptionOfResultComponent();

        // Act
        var title = component.Title;

        // Assert
        Assert.Equal(string.Empty, title);
    }

    [Fact]
    public void DisplayTitle_ShouldBeFalseWhenTitleIsEmpty()
    {
        // Arrange
        var component = new DescriptionOfResultComponent();
        component.Initialize(string.Empty);

        // Act
        var displayTitle = component.GetType().GetProperty("DisplayTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(component);

        // Assert
        Assert.False(displayTitle is bool displayTitleValue && displayTitleValue);
    }

    [Fact]
    public void DisplayTitle_ShouldBeTrueWhenTitleIsNotEmpty()
    {
        // Arrange
        var component = new DescriptionOfResultComponent();
        component.Initialize("Test Title");

        // Act
        var displayTitle = component.GetType().GetProperty("DisplayTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(component);

        // Assert
        Assert.True(displayTitle is bool displayTitleValue && displayTitleValue);
    }
}