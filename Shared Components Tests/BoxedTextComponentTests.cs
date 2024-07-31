using DBExplorerBlazor.Components;

namespace SharedComponents;

public class BoxedTextComponentTests
{
    [Fact]
    public void TextParameter_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var component = new BoxedTextComponent();
        var expectedText = "Sample Text";

        // Act
        component.Text = expectedText;
        var actualText = component.Text;

        // Assert
        Assert.Equal(expectedText, actualText);
    }

    [Fact]
    public void StyleParameter_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var component = new BoxedTextComponent();
        var expectedStyle = "color: red;";

        // Act
        component.Style = expectedStyle;
        var actualStyle = component.Style;

        // Assert
        Assert.Equal(expectedStyle, actualStyle);
    }
}