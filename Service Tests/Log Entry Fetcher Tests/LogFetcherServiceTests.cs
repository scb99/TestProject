using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace LogEntryFetcher;

public class LogFetcherServiceTests
{
    [Fact]
    public async Task FetchLogEntriesAsync_CallsDataManagerWithCorrectParameters()
    {
        // Arrange
        var mockDataManager = new Mock<IDataManager>();
        var service = new LogFetcherService(mockDataManager.Object);
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        var expectedLogEntries = new List<LogEntryEntity>
        {
            new() { /* Initialize properties as needed */ }
        };

        mockDataManager.Setup(dm => dm.GetLogEntriesSPAsync(startDate, endDate))
                       .ReturnsAsync(expectedLogEntries)
                       .Verifiable("DataManager was not called with the correct parameters.");

        // Act
        var result = await service.FetchLogEntriesAsync(startDate, endDate);

        // Assert
        mockDataManager.Verify(); // Verifies that IDataManager.GetLogEntriesSPAsync was called with the specified parameters
        Assert.Equal(expectedLogEntries, result); // Asserts that the method returns the expected result
    }

    [Fact]
    public async Task FetchLogEntriesAsync_ReturnsEmptyListWhenNoLogsFound()
    {
        // Arrange
        var mockDataManager = new Mock<IDataManager>();
        var service = new LogFetcherService(mockDataManager.Object);
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;

        mockDataManager.Setup(dm => dm.GetLogEntriesSPAsync(startDate, endDate))
                       .ReturnsAsync(new List<LogEntryEntity>());

        // Act
        var result = await service.FetchLogEntriesAsync(startDate, endDate);

        // Assert
        Assert.Empty(result); // Asserts that the method returns an empty list when no log entries are found
    }

    // Additional tests can be added here to cover more scenarios, such as handling exceptions thrown by IDataManager
}