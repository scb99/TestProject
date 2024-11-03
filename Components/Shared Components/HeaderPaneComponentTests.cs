using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class HeaderPaneComponentTests
{
    [Fact]
    public void CSSClass_ShouldBeHeaderPaneByDefault()
    {
        // Arrange
        var component = new HeaderPaneComponent();

        // Act
        var cssClass = component.CSSClass;

        // Assert
        Assert.Equal("headerPane", cssClass);
    }

    [Fact]
    public void CSSClass_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var component = new HeaderPaneComponent();
        var expectedCSSClass = "customHeaderPane";
        component.Initialize(null, expectedCSSClass);

        // Act
        var actualCSSClass = component.CSSClass;

        // Assert
        Assert.Equal(expectedCSSClass, actualCSSClass);
    }

    [Fact]
    public void ChildContent_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var component = new HeaderPaneComponent();
        var renderFragment = new RenderFragment(builder => builder.AddContent(0, "Test Content"));
        component.Initialize(renderFragment, "customHeaderPane");

        // Act
        var actualChildContent = component.ChildContent;

        // Assert
        Assert.Equal(renderFragment, actualChildContent);
    }

    [Fact]
    public void Initialize_ShouldSetChildContentAndCSSClass()
    {
        // Arrange
        var component = new HeaderPaneComponent();
        var renderFragment = new RenderFragment(builder => builder.AddContent(0, "Initialized Content"));
        var expectedCSSClass = "initializedHeaderPane";

        // Act
        component.Initialize(renderFragment, expectedCSSClass);

        // Assert
        Assert.Equal(renderFragment, component.ChildContent);
        Assert.Equal(expectedCSSClass, component.CSSClass);
    }
}