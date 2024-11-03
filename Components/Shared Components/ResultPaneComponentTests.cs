using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class ResultPaneComponentTests
{
    [Fact]
    public void ResultPaneComponent_DefaultCSSClass_IsResultPane()
    {
        // Arrange
        var resultPaneComponent = new ResultPaneComponent();

        // Act
        var cssClass = resultPaneComponent.CSSClass;

        // Assert
        Assert.Equal("resultPane", cssClass);
    }

    [Fact]
    public void ResultPaneComponent_Initialize_SetsPropertiesCorrectly()
    {
        // Arrange
        var resultPaneComponent = new ResultPaneComponent();
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));
        var cssClass = "customClass";

        // Act
        resultPaneComponent.Initialize(childContent, cssClass);

        // Assert
        Assert.Equal(childContent, resultPaneComponent.ChildContent);
        Assert.Equal(cssClass, resultPaneComponent.CSSClass);
    }

    [Fact]
    public void ResultPaneComponent_SetChildContent_UpdatesProperty()
    {
        // Arrange
        var resultPaneComponent = new ResultPaneComponent();
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));

        // Act
        //resultPaneComponent.ChildContent = childContent;
        resultPaneComponent.Initialize(childContent, "resultPane");

        // Assert
        Assert.Equal(childContent, resultPaneComponent.ChildContent);
    }

    [Fact]
    public void ResultPaneComponent_SetCSSClass_UpdatesProperty()
    {
        // Arrange
        var resultPaneComponent = new ResultPaneComponent();
        var cssClass = "customClass";

        // Act
        //resultPaneComponent.CSSClass = cssClass;
        resultPaneComponent.Initialize(new RenderFragment(builder => builder.AddContent(0, "Test Content")), cssClass);

        // Assert
        Assert.Equal(cssClass, resultPaneComponent.CSSClass);
    }
}