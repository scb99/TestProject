using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;

namespace SharedComponents;

public class GracePeriodSelectorComponentTests
{
    [Fact]
    public void OnAfterRender_ShouldSetGracePeriodDaysToMinusOneOnFirstRender()
    {
        // Arrange
        var component = new GracePeriodSelectorComponent();

        // Act
        component.OnAfterRender2(true);

        // Assert
        var gracePeriodDaysField = component.GetType().GetField("_gracePeriodDays", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var gracePeriodDaysValue = gracePeriodDaysField?.GetValue(component);
        Assert.Equal(-1, gracePeriodDaysValue);
    }

    [Fact]
    public async Task SubmitButtonClickedAsync_ShouldInvokeGracePeriodChangedIfPeriodChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var gracePeriodChanged = new EventCallback<int>();
        var component = new GracePeriodSelectorComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
        };
        component.Initialize(gracePeriodChanged);
        var gracePeriodField = component.GetType().GetField("_gracePeriod", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        gracePeriodField?.SetValue(component, "10 days");
        var gracePeriodDaysField = component.GetType().GetField("_gracePeriodDays", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        gracePeriodDaysField?.SetValue(component, 5);
        var gracePeriodChangedInvoked = false;
        component.Initialize(EventCallback.Factory.Create<int>(this, (value) =>
        {
            gracePeriodChangedInvoked = true;
        }));

        // Act
        await component.SubmitButtonClickedAsync();

        // Assert
        Assert.True(gracePeriodChangedInvoked);
    }

    [Fact]
    public async Task SubmitButtonClickedAsync_ShouldShowAlertIfPeriodNotChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var component = new GracePeriodSelectorComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
        };
        component.Initialize(EventCallback.Factory.Create<int>(this, (value) => Task.CompletedTask));
        var gracePeriodField = component.GetType().GetField("_gracePeriod", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        gracePeriodField?.SetValue(component, "10 days");
        var gracePeriodDaysField = component.GetType().GetField("_gracePeriodDays", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        gracePeriodDaysField?.SetValue(component, 10);

        // Act
        await component.SubmitButtonClickedAsync();

        // Assert
        mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("You didn't change the grace period!"), Times.Once);
    }

    [Fact]
    public async Task SubmitButtonClickedAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var component = new GracePeriodSelectorComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
        };
        component.Initialize(EventCallback.Factory.Create<int>(this, (value) => Task.CompletedTask));
        var gracePeriodField = component.GetType().GetField("_gracePeriod", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        gracePeriodField?.SetValue(component, "invalid period");

        // Act
        await component.SubmitButtonClickedAsync();

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}