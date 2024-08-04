using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace FindNewMembersData;

public class FindNewMembersDataServiceTests
{
    private readonly Mock<IDataManager> _mockDataManager;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly FindNewMembersDataService _service;

    public FindNewMembersDataServiceTests()
    {
        _mockDataManager = new Mock<IDataManager>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _service = new FindNewMembersDataService(_mockDataManager.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task FetchNewMembersDataAsync_ShouldReturnNewMembers()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var newMembers = new List<NewMemberEntity> { new() };

        _mockDataManager.Setup(dm => dm.GetNewMembersSPAsync(startDate, endDate))
            .ReturnsAsync(newMembers);

        // Act
        var result = await _service.FetchNewMembersDataAsync(startDate, endDate);

        // Assert
        Assert.Equal(newMembers, result);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task FetchNewMembersDataAsync_ShouldLogException_WhenExceptionThrown()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var exception = new Exception("Test exception");

        _mockDataManager.Setup(dm => dm.GetNewMembersSPAsync(startDate, endDate))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.FetchNewMembersDataAsync(startDate, endDate));
        Assert.Equal(exception, ex);
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, nameof(_service.FetchNewMembersDataAsync)), Times.Once);
    }
}