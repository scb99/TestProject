using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;

namespace SharedComponents;

public class GracePeriodSelectionScreenComponentTests
{
    [Fact]
    public void HeaderMessage_ShouldBeSetCorrectly()
    {
        // Arrange
        var component = new GracePeriodSelectionScreenComponent();
        var expectedHeaderMessage = "Test Header";

        // Act
        //component.HeaderMessage = expectedHeaderMessage;
        component.Initialize(expectedHeaderMessage, 0, EventCallback.Factory.Create<int>(this, (value) => { }));

        // Assert
        Assert.Equal(expectedHeaderMessage, component.HeaderMessage);
    }

    [Fact]
    public void GracePeriod_ShouldBeSetCorrectly()
    {
        // Arrange
        var component = new GracePeriodSelectionScreenComponent();
        var expectedGracePeriod = 10;

        // Act
        //component.GracePeriod = expectedGracePeriod;
        component.Initialize("Test Header", expectedGracePeriod, EventCallback.Factory.Create<int>(this, (value) => { })); 

        // Assert
        Assert.Equal(expectedGracePeriod, component.GracePeriod);
    }

    [Fact]
    public async Task ReceiveGracePeriodAsync_ShouldInvokeGracePeriodChanged()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var gracePeriodChanged = new EventCallback<int>();
        var component = new GracePeriodSelectionScreenComponent
        {
            Logger = mockLogger.Object
        };
        component.Initialize("Test Header", 0, gracePeriodChanged);
        var gracePeriod = 15;

        // Act
        var gracePeriodChangedInvoked = false;
        gracePeriodChanged = EventCallback.Factory.Create<int>(this, (value) =>
        {
            gracePeriodChangedInvoked = true;
        });
        component.Initialize("Test Header", 0, gracePeriodChanged);
        await component.ReceiveGracePeriodAsync(gracePeriod);

        // Assert
        Assert.True(gracePeriodChangedInvoked);
    }

    [Fact]
    public async Task ReceiveGracePeriodAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var gracePeriodChanged = EventCallback.Factory.Create<int>(this, (value) => throw new Exception("Test Exception"));
        var component = new GracePeriodSelectionScreenComponent
        {
            Logger = mockLogger.Object
        };
        component.Initialize("Test Header", 0, gracePeriodChanged);
        var gracePeriod = 15;

        // Act
        await component.ReceiveGracePeriodAsync(gracePeriod);

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}