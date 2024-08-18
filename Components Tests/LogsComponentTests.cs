using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class LogsComponentTests
{
    private readonly Mock<ICrossCuttingLoadingPanelService> _loadingPanelServiceMock;
    private readonly Mock<ILogEntryFetcher> _logEntryFetcherMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<INewMemberTitleGeneratorService> _newMemberTitleGeneratorMock;
    private readonly LogsComponent _component;

    public LogsComponentTests()
    {
        _loadingPanelServiceMock = new Mock<ICrossCuttingLoadingPanelService>();
        _logEntryFetcherMock = new Mock<ILogEntryFetcher>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _newMemberTitleGeneratorMock = new Mock<INewMemberTitleGeneratorService>();

        _component = new LogsComponent
        {
            LoadingPanelService = _loadingPanelServiceMock.Object,
            LogEntryFetcher = _logEntryFetcherMock.Object,
            Logger = _loggerMock.Object,
            NewMemberTitleGenerator = _newMemberTitleGeneratorMock.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldFetchLogEntriesAndSetTitle()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var logEntries = new List<LogEntryEntity>
        {
            new() { UserID = 1, Message = "Log 1" },
            new() { UserID = 2, Message = "Log 2" }
        };
        var expectedTitle = "2 logs found between 01/01/2023 and 12/31/2023";

        _logEntryFetcherMock
            .Setup(fetcher => fetcher.FetchLogEntriesAsync(startDate, endDate))
            .ReturnsAsync(logEntries);

        _newMemberTitleGeneratorMock
            .Setup(generator => generator.GenerateNewMemberTitle(logEntries.Count, startDate, endDate))
            .Returns(expectedTitle);

        _component.StartDate = startDate;
        _component.EndDate = endDate;

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _loadingPanelServiceMock.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        _logEntryFetcherMock.Verify(fetcher => fetcher.FetchLogEntriesAsync(startDate, endDate), Times.Once);
        _newMemberTitleGeneratorMock.Verify(generator => generator.GenerateNewMemberTitle(logEntries.Count, startDate, endDate), Times.Once);
        Assert.Equal(logEntries, _component.LogEntitiesBDP);
        Assert.Equal(expectedTitle, _component.TitleBDP);
        Assert.False(_component.LoadingBDP);
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldHandleException()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var exception = new Exception("Test exception");

        _logEntryFetcherMock
            .Setup(fetcher => fetcher.FetchLogEntriesAsync(startDate, endDate))
            .ThrowsAsync(exception);

        _component.StartDate = startDate;
        _component.EndDate = endDate;

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _loggerMock.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
        Assert.False(_component.LoadingBDP);
    }
}
