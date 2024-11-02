using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DownloadButtonReferenceComponentTests
{
    [Fact]
    public void Text_ShouldBeNullByDefault()
    {
        // Arrange
        var component = new DownloadButtonReferenceComponent();

        // Act
        var text = component.Text;

        // Assert
        Assert.Null(text);
    }

    [Fact]
    public void Style_ShouldBeNullByDefault()
    {
        // Arrange
        var component = new DownloadButtonReferenceComponent();

        // Act
        var style = component.Style;

        // Assert
        Assert.Null(style);
    }

    [Fact]
    public void Initialize_ShouldSetTextAndStyle()
    {
        // Arrange
        var component = new DownloadButtonReferenceComponent();
        var expectedText = "Download";
        var expectedStyle = "btn-primary";

        // Act
        component.Initialize(expectedText, expectedStyle);

        // Assert
        Assert.Equal(expectedText, component.Text);
        Assert.Equal(expectedStyle, component.Style);
    }
}