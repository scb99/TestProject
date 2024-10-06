using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberCleanup;

public class MemberCleanupServiceTests
{
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<IRepositoryUsermeta> _mockDeleteUsermetaRecords;
    private readonly Mock<IRepositoryUsers> _mockDeleteUserTableRecord;
    private readonly Mock<IRepositoryMembersWithNoFirstAndLastName> _mockGetMembersWithNoFirstAndLastName;
    private readonly MemberCleanupService _service;

    public MemberCleanupServiceTests()
    {
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockDeleteUsermetaRecords = new Mock<IRepositoryUsermeta>();
        _mockDeleteUserTableRecord = new Mock<IRepositoryUsers>();
        _mockGetMembersWithNoFirstAndLastName = new Mock<IRepositoryMembersWithNoFirstAndLastName>();

        _service = new MemberCleanupService(
            _mockLoggerService.Object,
            _mockDeleteUsermetaRecords.Object,
            _mockDeleteUserTableRecord.Object,
            _mockGetMembersWithNoFirstAndLastName.Object);
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsAsync_NoBadRecords_LogsAndReturnsZero()
    {
        // Arrange
        _mockGetMembersWithNoFirstAndLastName
            .Setup(s => s.GetMembersWithNoFirstAndLastNameAsync())
            .ReturnsAsync(new List<UserIDsEntity>());

        // Act
        var result = await _service.RemoveBadMemberDataRecordsAsync();

        // Assert
        Assert.Equal(0, result);
        _mockLoggerService.Verify(l => l.LogResultAsync("No bad member data records found to remove."), Times.Once);
    }

    [Fact]
    public async Task RemoveBadMemberDataRecordsAsync_BadRecordsExist_DeletesRecordsAndLogs()
    {
        // Arrange
        var badMembers = new List<UserIDsEntity>
        {
            new() { ID = 1 },
            new() { ID = 2 }
        };

        _mockGetMembersWithNoFirstAndLastName
            .Setup(s => s.GetMembersWithNoFirstAndLastNameAsync())
            .ReturnsAsync(badMembers);

        _mockDeleteUsermetaRecords
            .Setup(s => s.DeleteUsermetaRecordsAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(2));

        _mockDeleteUserTableRecord
            .Setup(s => s.DeleteUsersRecordAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(2));

        // Act
        var result = await _service.RemoveBadMemberDataRecordsAsync();

        // Assert
        Assert.Equal(2, result);
        _mockDeleteUsermetaRecords.Verify(d => d.DeleteUsermetaRecordsAsync(It.IsAny<int>()), Times.Exactly(2));
        _mockDeleteUserTableRecord.Verify(d => d.DeleteUsersRecordAsync(It.IsAny<int>()), Times.Exactly(2));
        _mockLoggerService.Verify(l => l.LogResultAsync("Removed 2 bad member data records with IDs: 1, 2"), Times.Once);
    }
}
