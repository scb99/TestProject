using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DescriptionOfHeaderComponentTests
{
    [Fact]
    public void Title_ShouldBeEmptyStringByDefault()
    {
        // Arrange
        var component = new DescriptionOfHeaderComponent();

        // Act
        var title = component.Title;

        // Assert
        Assert.Equal(string.Empty, title);
    }

    [Fact]
    public void Logo_ShouldBeTrueByDefault()
    {
        // Arrange
        var component = new DescriptionOfHeaderComponent();

        // Act
        var logo = component.Logo;

        // Assert
        Assert.True(logo);
    }

    [Fact]
    public void DisplayTitle_ShouldBeFalseWhenTitleIsEmpty()
    {
        // Arrange
        var component = new DescriptionOfHeaderComponent();
        component.Initialize(string.Empty, true);

        // Act
        var displayTitle = component.GetType().GetProperty("DisplayTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(component);

        // Assert
        Assert.False(displayTitle is bool displayTitleValue && displayTitleValue);
    }

    [Fact]
    public void DisplayTitle_ShouldBeTrueWhenTitleIsNotEmpty()
    {
        // Arrange
        var component = new DescriptionOfHeaderComponent();
        component.Initialize("Test Title", true);

        // Act
        var displayTitle = component.GetType().GetProperty("DisplayTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(component);

        // Assert
        Assert.True(displayTitle is bool displayTitleValue && displayTitleValue);
    }
}