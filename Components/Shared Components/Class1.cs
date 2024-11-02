using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DownloadTXTFileComponentTests
{
    [Fact]
    public void OnClick_ShouldBeDefaultEventCallback()
    {
        // Arrange
        var component = new DownloadTXTFileComponent();

        // Act
        var onClick = component.OnClick;

        // Assert
        Assert.False(onClick.HasDelegate);
    }
}