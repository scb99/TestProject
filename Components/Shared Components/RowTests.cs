using DBExplorerBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace SharedComponents;

public class RowTests
{
    [Fact]
    public void Row_Initialize_SetsChildContent()
    {
        // Arrange
        var row = new Row();
        var childContent = new RenderFragment(builder => builder.AddContent(0, "Test Content"));

        // Act
        row.Initialize(childContent);

        // Assert
        Assert.Equal(childContent, row.ChildContent);
    }

    [Fact]
    public void Row_ChildContent_DefaultsToNull()
    {
        // Arrange
        var row = new Row();

        // Act
        var childContent = row.ChildContent;

        // Assert
        Assert.Null(childContent);
    }
}