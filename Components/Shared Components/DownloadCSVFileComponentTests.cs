using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DownloadCSVFileComponentTests
{
    [Fact]
    public void OnClick_ShouldBeDefaultEventCallback()
    {
        // Arrange
        var component = new DownloadCSVFileComponent();

        // Act
        var onClick = component.OnClick;

        // Assert
        Assert.False(onClick.HasDelegate);
    }
}