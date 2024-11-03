using DBExplorerBlazor.Components;

namespace SharedComponents;

public class TextComponentTests
{
    [Fact]
    public void TextComponent_Initialize_SetsPropertiesCorrectly()
    {
        // Arrange
        var textComponent = new TextComponent();
        var text = "Sample Text";
        var style = "color: blue;";

        // Act
        textComponent.Initialize(text, style);

        // Assert
        Assert.Equal(text, textComponent.Text);
        Assert.Equal(style, textComponent.Style);
    }

    [Fact]
    public void TextComponent_Text_DefaultsToNull()
    {
        // Arrange
        var textComponent = new TextComponent();

        // Act
        var text = textComponent.Text;

        // Assert
        Assert.Null(text);
    }

    [Fact]
    public void TextComponent_Style_DefaultsToNull()
    {
        // Arrange
        var textComponent = new TextComponent();

        // Act
        var style = textComponent.Style;

        // Assert
        Assert.Null(style);
    }
}