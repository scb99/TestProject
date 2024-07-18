using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberCleanupServiceTests
{
    private readonly Mock<IDataManager> _mockDataManager = new();
    private readonly Mock<ILoggerService> _mockLoggerService = new();
    private readonly MemberCleanupService _memberCleanupService;

    public MemberCleanupServiceTests()
    {
        _memberCleanupService = new MemberCleanupService(_mockDataManager.Object, _mockLoggerService.Object);
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsAsync_NoRecordsFound_ReturnsZero()
    {
        // Arrange
        _mockDataManager.Setup(dm => dm.GetMembersWithNoFirstAndLastNameSPAsync()).ReturnsAsync(new List<UserIDsEntity>());

        // Act
        var result = await _memberCleanupService.RemoveBadMemberDataRecordsAsync();

        // Assert
        Assert.Equal(0, result);
        _mockLoggerService.Verify(ls => ls.LogResultAsync("No bad member data records found to remove."), Times.Once);
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsAsync_RecordsFound_ReturnsCount()
    {
        // Arrange
        var userIDs = new List<UserIDsEntity> { new() { ID = 1 }, new() { ID = 2 } };
        _mockDataManager.Setup(dm => dm.GetMembersWithNoFirstAndLastNameSPAsync()).ReturnsAsync(userIDs);
        _mockDataManager.Setup(dm => dm.DeleteUsermetaRecordsAsync(It.IsAny<int>())).Returns(Task.FromResult(0));
        _mockDataManager.Setup(dm => dm.DeleteUserTableRecordAsync(It.IsAny<int>())).Returns(Task.FromResult(0));

        // Act
        var result = await _memberCleanupService.RemoveBadMemberDataRecordsAsync();

        // Assert
        Assert.Equal(userIDs.Count, result);
        _mockLoggerService.Verify(ls => ls.LogResultAsync(It.Is<string>(s => s.Contains("Removed 2 bad member data records"))), Times.Once);
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsAsync_ExceptionThrown_LogsError()
    {
        // Arrange
        _mockDataManager.Setup(dm => dm.GetMembersWithNoFirstAndLastNameSPAsync()).ThrowsAsync(new System.Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => _memberCleanupService.RemoveBadMemberDataRecordsAsync());
        _mockLoggerService.Verify(ls => ls.LogExceptionAsync(It.IsAny<System.Exception>(), It.IsAny<string>()), Times.Never); // Assuming LogExceptionAsync is the method to log exceptions
    }
}