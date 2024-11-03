using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class SpanComponentTests
{
    [Fact]
    public void SpanComponent_Initialize_SetsPropertiesCorrectly()
    {
        // Arrange
        var spanComponent = new SpanComponent();
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));
        var style = "color: red;";

        // Act
        spanComponent.Initialize(childContent, style);

        // Assert
        Assert.Equal(childContent, spanComponent.ChildContent);
        Assert.Equal(style, spanComponent.Style);
    }

    [Fact]
    public void SpanComponent_ChildContent_DefaultsToNull()
    {
        // Arrange
        var spanComponent = new SpanComponent();

        // Act
        var childContent = spanComponent.ChildContent;

        // Assert
        Assert.Null(childContent);
    }

    [Fact]
    public void SpanComponent_Style_DefaultsToNull()
    {
        // Arrange
        var spanComponent = new SpanComponent();

        // Act
        var style = spanComponent.Style;

        // Assert
        Assert.Null(style);
    }
}