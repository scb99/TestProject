using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class LoadingComponentTests
{
    [Fact]
    public void LoadingComponent_DefaultLoadingText_IsLoading()
    {
        // Arrange
        var loadingComponent = new LoadingComponent();

        // Act
        var loadingText = loadingComponent.LoadingText;

        // Assert
        Assert.Equal("Loading...", loadingText);
    }

    [Fact]
    public void LoadingComponent_Initialize_SetsPropertiesCorrectly()
    {
        // Arrange
        var loadingComponent = new LoadingComponent();
        var isLoading = true;
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));
        var loadingText = "Please wait...";

        // Act
        loadingComponent.Initialize(isLoading, childContent, loadingText);

        // Assert
        Assert.True(loadingComponent.IsLoading);
        Assert.Equal(childContent, loadingComponent.ChildContent);
        Assert.Equal(loadingText, loadingComponent.LoadingText);
    }

    [Fact]
    public void LoadingComponent_SetIsLoading_UpdatesProperty()
    {
        // Arrange
        var loadingComponent = new LoadingComponent();

        // Act
        loadingComponent.Initialize(true, null, null);

        // Assert
        Assert.True(loadingComponent.IsLoading);
    }

    [Fact]
    public void LoadingComponent_SetChildContent_UpdatesProperty()
    {
        // Arrange
        var loadingComponent = new LoadingComponent();
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));

        // Act
        loadingComponent.Initialize(false, childContent, null);

        // Assert
        Assert.Equal(childContent, loadingComponent.ChildContent);
    }

    [Fact]
    public void LoadingComponent_SetLoadingText_UpdatesProperty()
    {
        // Arrange
        var loadingComponent = new LoadingComponent();
        var loadingText = "Please wait...";

        // Act
        loadingComponent.Initialize(false, null, loadingText);

        // Assert
        Assert.Equal(loadingText, loadingComponent.LoadingText);
    }
}