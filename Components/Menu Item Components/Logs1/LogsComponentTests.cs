using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
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

        _component = new LogsComponent();

        _component.SetPrivatePropertyValue("LoadingPanelService", _mockLoadingPanelService.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLoggerService.Object);
        _component.SetPrivatePropertyValue("LogRepository", _mockLogRepository.Object);
        _component.SetPrivatePropertyValue("LogTitleGenerator", _mockLogTitleGenerator.Object);

        _component.SetPublicPropertyValue("StartDate", DateTime.Now.AddDays(-30));
        _component.SetPublicPropertyValue("EndDate", DateTime.Now);
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
        await typeof(LogsComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(logs, _component.GetPrivatePropertyValue<List<LogEntryEntity>>("LogEntitiesBDP"));
        Assert.Equal("Generated Title", _component.GetPrivatePropertyValue<string>("TitleBDP"));
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
        await typeof(LogsComponent).InvokeAsync("LoadLogsAndManageUIAsync", _component);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
        Assert.Equal(logs, _component.GetPrivatePropertyValue<List<LogEntryEntity>>("LogEntitiesBDP"));
        Assert.Equal("Generated Title", _component.GetPrivatePropertyValue<string>("TitleBDP"));
    }

    [Fact]
    public async Task LoadLogsAndManageUIAsync_ShowsAndHidesLoadingPanel()
    {
        // Arrange
        var logs = new List<LogEntryEntity> { new() };
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ReturnsAsync(logs);

        // Act
        await typeof(LogsComponent).InvokeAsync("LoadLogsAndManageUIAsync", _component);

        // Assert
        _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
    }

    [Fact]
    public async Task LoadLogsAndManageUIAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                          .ThrowsAsync(exception);

        // Act
        await typeof(LogsComponent).InvokeAsync("LoadLogsAndManageUIAsync", _component);

        // Assert
        _mockLoggerService.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
    }
}