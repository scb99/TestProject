using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace DBExplorerBlazor.Tests.Components
{
    public class LogsComponentTests
    {
        private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
        private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
        private readonly Mock<IRepositoryLogEntriesByDateRange> _mockLogRepository;
        private readonly Mock<ILogTitleGeneratorService> _mockNewMemberTitleGenerator;
        private readonly LogsComponent _component;

        public LogsComponentTests()
        {
            _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
            _mockLogger = new Mock<ICrossCuttingLoggerService>();
            _mockLogRepository = new Mock<IRepositoryLogEntriesByDateRange>();
            _mockNewMemberTitleGenerator = new Mock<ILogTitleGeneratorService>();

            _component = new LogsComponent
            {
                LoadingPanelService = _mockLoadingPanelService.Object,
                Logger = _mockLogger.Object,
                LogRepository = _mockLogRepository.Object,
                LogTitleGenerator = _mockNewMemberTitleGenerator.Object,
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 1, 31)
            };
        }

        [Fact]
        public async Task OnParametersSetAsync_LoadsLogEntriesAndSetsTitle()
        {
            // Arrange
            var logEntries = new List<LogEntryEntity>
            {
                new() { UserID = 1, Message = "Log Entry 1", TimeStamp = new DateTime(2023, 1, 10) },
                new() { UserID = 2, Message = "Log Entry 2", TimeStamp = new DateTime(2023, 1, 20) }
            };
            var title = "Title";

            _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                .ReturnsAsync(logEntries);
            _mockNewMemberTitleGenerator.Setup(generator => generator.GenerateLogTitle(logEntries.Count, _component.StartDate, _component.EndDate))
                .Returns(title);

            // Act
            await _component.OnParametersSet2Async();

            // Assert
            Assert.Equal(logEntries, _component.LogEntitiesBDP);
            Assert.Equal(title, _component.TitleBDP);
            _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
            _mockLogger.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task OnParametersSetAsync_LogsExceptionOnFailure()
        {
            // Arrange
            var exception = new Exception("Test exception");

            _mockLogRepository.Setup(repo => repo.GetAllLogEntriesByDateRangeAsync(_component.StartDate, _component.EndDate))
                .ThrowsAsync(exception);

            // Act
            await _component.OnParametersSet2Async();

            // Assert
            _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
            _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        }
    }
}
