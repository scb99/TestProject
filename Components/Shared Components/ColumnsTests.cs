using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class ColumnsTests
{
    private readonly Columns _component;

    public ColumnsTests()
    {
        _component = new Columns();
    }

    [Fact]
    public void Columns_ShouldSetNumParameter()
    {
        // Arrange
        _component.Initialize("4", string.Empty, string.Empty, null);

        // Act
        var numField = _component.GetType().GetProperty("Num", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var numValue = numField?.GetValue(_component)?.ToString();

        // Assert
        Assert.Equal("4", numValue);
    }

    [Fact]
    public void Columns_ShouldSetCSSClassParameter()
    {
        // Arrange
        _component.Initialize(string.Empty, "custom-class", string.Empty, null);

        // Act
        var cssClassField = _component.GetType().GetProperty("CSSClass", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var cssClassValue = cssClassField?.GetValue(_component)?.ToString();

        // Assert
        Assert.Equal("custom-class", cssClassValue);
    }

    [Fact]
    public void Columns_ShouldSetStyleParameter()
    {
        // Arrange
        _component.Initialize(string.Empty, string.Empty, "color: red;", null);

        // Act
        var styleField = _component.GetType().GetProperty("Style", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var styleValue = styleField?.GetValue(_component)?.ToString();

        // Assert
        Assert.Equal("color: red;", styleValue);
    }

    [Fact]
    public void Columns_ShouldSetChildContentParameter()
    {
        // Arrange
        var renderFragment = new RenderFragment(builder => builder.AddContent(0, "Child content"));
        _component.Initialize(string.Empty, string.Empty, string.Empty, renderFragment);

        // Act
        var childContentField = _component.GetType().GetProperty("ChildContent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var childContentValue = childContentField?.GetValue(_component) as RenderFragment;

        // Assert
        Assert.NotNull(childContentValue);
    }

    [Fact]
    public void Columns_ShouldGenerateCorrectCSS()
    {
        // Arrange
        _component.Initialize("4", "custom-class", string.Empty, null);

        // Act
        var cssField = _component.GetType().GetProperty("CSS", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var cssValue = cssField?.GetValue(_component)?.ToString();

        // Assert
        Assert.Equal("col-md-4 custom-class", cssValue);
    }
}