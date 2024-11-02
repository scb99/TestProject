using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DownloadExcelFileComponentTests
{
    [Fact]
    public void OnClick_ShouldBeDefaultEventCallback()
    {
        // Arrange
        var component = new DownloadExcelFileComponent();

        // Act
        var onClick = component.OnClick;

        // Assert
        Assert.False(onClick.HasDelegate);
    }
}