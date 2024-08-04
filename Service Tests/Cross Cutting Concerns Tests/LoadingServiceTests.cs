using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class LoadingServiceTests
{
    private readonly Mock<ICrossCuttingLoadingStateService> _mockLoadingStateService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly LoadingService _loadingService;

    public LoadingServiceTests()
    {
        _mockLoadingStateService = new Mock<ICrossCuttingLoadingStateService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _loadingService = new LoadingService(_mockLoadingStateService.Object, _mockLoggerService.Object);
    }

    [Fact]
    public async Task ExecuteWithLoadingStateAsync_ShouldShowAndHideLoadingPanel()
    {
        // Arrange
        var action = new Func<Task>(() => Task.CompletedTask);
        var onLoadingStateChanged = new Action<bool>(isLoading => { });

        // Act
        await _loadingService.ShowSpinnersExecuteHideSpinnersAsync(action, onLoadingStateChanged);

        // Assert
        _mockLoadingStateService.Verify(s => s.ShowLoadingPanel(), Times.Once);
        _mockLoadingStateService.Verify(s => s.HideLoadingPanel(), Times.Once);
    }

    [Fact]
    public async Task ExecuteWithLoadingStateAsync_ShouldInvokeAction()
    {
        // Arrange
        var actionInvoked = false;
        var action = new Func<Task>(() =>
        {
            actionInvoked = true;
            return Task.CompletedTask;
        });
        var onLoadingStateChanged = new Action<bool>(isLoading => { });

        // Act
        await _loadingService.ShowSpinnersExecuteHideSpinnersAsync(action, onLoadingStateChanged);

        // Assert
        Assert.True(actionInvoked);
    }

    [Fact]
    public async Task ExecuteWithLoadingStateAsync_ShouldLogException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var action = new Func<Task>(() => throw exception);
        var onLoadingStateChanged = new Action<bool>(isLoading => { });

        // Act
        await _loadingService.ShowSpinnersExecuteHideSpinnersAsync(action, onLoadingStateChanged);

        // Assert
        _mockLoggerService.Verify(s => s.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteWithLoadingStateAsync_ShouldAddAndRemoveOnLoadingStateChanged()
    {
        // Arrange
        var action = new Func<Task>(() => Task.CompletedTask);
        var onLoadingStateChanged = new Action<bool>(isLoading => { });

        // Act
        await _loadingService.ShowSpinnersExecuteHideSpinnersAsync(action, onLoadingStateChanged);

        // Assert
        _mockLoadingStateService.VerifyAdd(s => s.OnLoadingStateChanged += onLoadingStateChanged, Times.Once);
        _mockLoadingStateService.VerifyRemove(s => s.OnLoadingStateChanged -= onLoadingStateChanged, Times.Once);
    }
}