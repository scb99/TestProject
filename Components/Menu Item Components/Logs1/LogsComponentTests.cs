using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class LogsComponentTests
{
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<IRepositoryLogEntriesByDateRange> _mockLogRepository;
    private readonly Mock<ILogTitleGeneratorService> _mockLogTitleGenerator;
    private readonly LogsComponent _component;

    public LogsComponentTests()
    {
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockLogRepository = new Mock<IRepositoryLogEntriesByDateRange>();
        _mockLogTitleGenerator = new Mock<ILogTitleGeneratorService>();

        _component = new LogsComponent
        {
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLoggerService.Object,
            LogRepository = _mockLogRepository.Object,
            LogTitleGenerator = _mockLogTitleGenerator.Object,
        };

        _component.Initialize(DateTime.Now.AddDays(-30), DateTime.Now);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsLogEntitiesAndTitle()
    {
        // Arrange
        var logs = new List<LogEntryEntity> { new() };
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ReturnsAsync(logs);
        _mockLogTitleGenerator.Setup(generator => generator.GenerateLogTitle(logs.Count, _component.StartDate, _component.EndDate))
                              .Returns("Generated Title");

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(logs, _component.LogEntitiesBDP);
        Assert.Equal("Generated Title", _component.TitleBDP);
    }

    [Fact]
    public async Task LoadLogsAndManageUIAsync_SetsLoadingStateCorrectly()
    {
        // Arrange
        var logs = new List<LogEntryEntity> { new() };
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ReturnsAsync(logs);
        _mockLogTitleGenerator.Setup(generator => generator.GenerateLogTitle(logs.Count, _component.StartDate, _component.EndDate))
                              .Returns("Generated Title");

        // Act
        await _component.LoadLogsAndManageUIAsync();

        // Assert
        Assert.False(_component.LoadingBDP);
        Assert.Equal(logs, _component.LogEntitiesBDP);
        Assert.Equal("Generated Title", _component.TitleBDP);
    }

    [Fact]
    public async Task LoadLogsAndManageUIAsync_ShowsAndHidesLoadingPanel()
    {
        // Arrange
        var logs = new List<LogEntryEntity> { new() };
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ReturnsAsync(logs);

        // Act
        await _component.LoadLogsAndManageUIAsync();

        // Assert
        _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.LoadingBDP);
    }

    [Fact]
    public async Task LoadLogsAndManageUIAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ThrowsAsync(exception);

        // Act
        await _component.LoadLogsAndManageUIAsync();

        // Assert
        _mockLoggerService.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
        Assert.False(_component.LoadingBDP);
    }
}