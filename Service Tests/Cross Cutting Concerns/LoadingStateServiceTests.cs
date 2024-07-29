using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class LoadingStateServiceTests
{
    [Fact]
    public void ShowLoadingPanel_RaisesOnLoadingStateChangedWithTrue()
    {
        // Arrange
        var service = new LoadingStateService();
        bool? eventRaisedWith = null;
        service.OnLoadingStateChanged += (isLoading) => eventRaisedWith = isLoading;

        // Act
        service.ShowLoadingPanel();

        // Assert
        Assert.True(eventRaisedWith.HasValue && eventRaisedWith.Value);
    }

    [Fact]
    public void HideLoadingPanel_RaisesOnLoadingStateChangedWithFalse()
    {
        // Arrange
        var service = new LoadingStateService();
        bool? eventRaisedWith = null;
        service.OnLoadingStateChanged += (isLoading) => eventRaisedWith = isLoading;

        // Act
        service.HideLoadingPanel();

        // Assert
        Assert.True(eventRaisedWith.HasValue && !eventRaisedWith.Value);
    }
}