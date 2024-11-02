using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace SharedComponents;

public class DuesGridComponentTests
{
    [Fact]
    public async Task ShowMeAsync_ShouldSetDuesEntitiesBDP()
    {
        // Arrange
        var mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        mockExecute.Setup(e => e.ConditionalCode()).Returns(false);
        var component = new DuesGridComponent
        {
            Execute = mockExecute.Object
        };
        var duesEntities = new List<DuesEntity>
        {
            new() { /* Initialize properties */ },
            new() { /* Initialize properties */ }
        };

        // Act
        await component.ShowMeAsync(duesEntities);

        // Assert
        var duesEntitiesBDPField = component.GetType().GetProperty("DuesEntitiesBDP", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var duesEntitiesBDPValue = duesEntitiesBDPField?.GetValue(component) as List<DuesEntity>;
        Assert.Equal(duesEntities, duesEntitiesBDPValue);
    }
}