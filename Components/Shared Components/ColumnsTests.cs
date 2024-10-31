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
        var columns = new Columns
        {
#pragma warning disable BL0005
            Num = "4"
#pragma warning restore BL0005
        };

        // Act
        var numField = columns.GetType().GetProperty("Num", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var numValue = numField?.GetValue(columns)?.ToString();

        // Assert
        Assert.Equal("4", numValue);
    }

    [Fact]
    public void Columns_ShouldSetCSSClassParameter()
    {
        // Arrange
        var columns = new Columns
        {
#pragma warning disable BL0005
            CSSClass = "custom-class"
#pragma warning restore BL0005
        };

        // Act
        var cssClassField = columns.GetType().GetProperty("CSSClass", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var cssClassValue = cssClassField?.GetValue(columns)?.ToString();

        // Assert
        Assert.Equal("custom-class", cssClassValue);
    }

    [Fact]
    public void Columns_ShouldSetStyleParameter()
    {
        // Arrange
        var columns = new Columns
        {
#pragma warning disable BL0005
            Style = "color: red;"
#pragma warning restore BL0005
        };

        // Act
        var styleField = columns.GetType().GetProperty("Style", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var styleValue = styleField?.GetValue(columns)?.ToString();

        // Assert
        Assert.Equal("color: red;", styleValue);
    }

    [Fact]
    public void Columns_ShouldSetChildContentParameter()
    {
        // Arrange
        var renderFragment = new RenderFragment(builder => builder.AddContent(0, "Child content"));
        var columns = new Columns
        {
#pragma warning disable BL0005
            ChildContent = renderFragment
#pragma warning restore BL0005
        };

        // Act
        var childContentField = columns.GetType().GetProperty("ChildContent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var childContentValue = childContentField?.GetValue(columns) as RenderFragment;

        // Assert
        Assert.NotNull(childContentValue);
    }

    [Fact]
    public void Columns_ShouldGenerateCorrectCSS()
    {
        // Arrange
        var columns = new Columns
        {
#pragma warning disable BL0005
            CSSClass = "custom-class",
            Num = "4"
#pragma warning restore BL0005
        };

        // Act
        var cssField = columns.GetType().GetProperty("CSS", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var cssValue = cssField?.GetValue(columns)?.ToString();

        // Assert
        Assert.Equal("col-md-4 custom-class", cssValue);
    }
}