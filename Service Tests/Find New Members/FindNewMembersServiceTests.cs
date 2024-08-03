using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace FindNewMembers;

public class FindNewMembersServiceTests
{
    private readonly Mock<IDataManager> mockDataManager;
    private readonly Mock<ICrossCuttingLoggerService> mockLogger;
    private readonly FindNewMembersService findNewMembersService;

    public FindNewMembersServiceTests()
    {
        mockDataManager = new Mock<IDataManager>();
        mockLogger = new Mock<ICrossCuttingLoggerService>();
        findNewMembersService = new FindNewMembersService(mockDataManager.Object, mockLogger.Object);
    }

    [Fact]
    public async Task TryFetchAndDisplayNewMembersAsync_ReturnsNewMembers()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 1, 31);
        var newMembers = new List<NewMemberEntity>
        {
            new() { ID = 1, Name = "John Doe" },
            new() { ID = 2, Name = "Jane Smith" }
        };

        mockDataManager.Setup(dm => dm.GetNewMembersSPAsync(startDate, endDate))
                       .ReturnsAsync(newMembers);

        // Act
        var result = await findNewMembersService.TryFetchAndDisplayNewMembersAsync(startDate, endDate);

        // Assert
        Assert.Equal(newMembers, result);
        mockLogger.Verify(logger => logger.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task TryFetchAndDisplayNewMembersAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 1, 31);
        var exception = new Exception("Database error");

        mockDataManager.Setup(dm => dm.GetNewMembersSPAsync(startDate, endDate))
                       .ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => findNewMembersService.TryFetchAndDisplayNewMembersAsync(startDate, endDate));
        mockLogger.Verify(logger => logger.LogExceptionAsync(exception, nameof(findNewMembersService.TryFetchAndDisplayNewMembersAsync)), Times.Once);
    }
}