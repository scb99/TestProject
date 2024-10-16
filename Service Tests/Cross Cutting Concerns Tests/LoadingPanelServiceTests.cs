﻿using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class LoadingPanelServiceTests
{
    private readonly LoadingPanelService _loadingPanelService;

    public LoadingPanelServiceTests()
    {
        _loadingPanelService = new LoadingPanelService();
    }

    [Fact]
    public async Task ShowLoadingPanelAsync_ShouldCompleteAfterDelay()
    {
        // Act
        var task = _loadingPanelService.ShowLoadingPanelAsync();

        // Assert
        var delayTask = Task.Delay(1500); // Slightly longer than the delay in the method
        var completedTask = await Task.WhenAny(task, delayTask);

        Assert.Equal(task, completedTask);
    }
}