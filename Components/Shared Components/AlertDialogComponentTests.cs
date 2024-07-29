using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace SharedComponents;

public class AlertDialogComponentTests
{
    [Fact]
    public void ComponentSubscribesAndUnsubscribesToAlertServiceOnInitializedAndDispose()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var component = new AlertDialogComponent { AlertService = mockAlertService.Object };

        // Act
        component.OnInitialized2();
        component.Dispose();

        // Assert
        mockAlertService.Verify(service => service.Subscribe(It.IsAny<Func<string, Task>>()), Times.Once);
        mockAlertService.Verify(service => service.Unsubscribe(It.IsAny<Func<string, Task>>()), Times.Once);
    }

    [Fact]
    public async Task OnShowAlertDialogBoxAsync_UpdatesStateToShowDialog()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var component = new AlertDialogComponent { AlertService = mockAlertService.Object };
        string testMessage = "Test Alert Message";

        // Act
        await component.OnShowAlertDialogBox2Async(testMessage);

        // Assert
        Assert.True(component.IsVisibleBDP);
        Assert.Equal(testMessage, component.MessageBDP);
    }

    [Fact]
    public async Task OnDismissDialogButtonClickedAsync_UpdatesStateToHideDialog()
    {
        // Arrange
        var component = new AlertDialogComponent
        {
            IsVisibleBDP = true, // Simulate dialog being shown
            MessageBDP = "Test Alert Message"
        };

        // Act
        await component.OnDismissDialogButtonClicked2Async();

        // Assert
        Assert.False(component.IsVisibleBDP);
    }
}
