using DBExplorerBlazor.Components;

namespace SharedComponents;

public class DownloadButtonComponentTests
{
    [Fact]
    public void ToolTip_ShouldBeNullByDefault()
    {
        // Arrange
        var component = new DownloadButtonComponent();

        // Act
        var toolTip = component.ToolTip;

        // Assert
        Assert.Null(toolTip);
    }

    [Fact]
    public void Export_ShouldBeNullByDefault()
    {
        // Arrange
        var component = new DownloadButtonComponent();

        // Act
        var export = component.Export;

        // Assert
        Assert.Null(export);
    }

    [Fact]
    public void OnClick_ShouldBeDefaultEventCallback()
    {
        // Arrange
        var component = new DownloadButtonComponent();

        // Act
        var onClick = component.OnClick;

        // Assert
        Assert.False(onClick.HasDelegate);
    }

    [Fact]
    public void ToolTip_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var component = new DownloadButtonComponent();
        var expectedToolTip = "Download File";

        // Act
        component.Initialize(expectedToolTip, "");
        var actualToolTip = component.ToolTip;

        // Assert
        Assert.Equal(expectedToolTip, actualToolTip);
    }

    [Fact]
    public void Export_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var component = new DownloadButtonComponent();
        var expectedExport = "ExportData";

        // Act
        component.Initialize("", expectedExport);
        var actualExport = component.Export;

        // Assert
        Assert.Equal(expectedExport, actualExport);
    }
}