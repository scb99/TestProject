using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class LoggingMemberSelectionServiceDecoratorTests
{
    [Fact]
    public async Task ProcessSelectedMemberAsync_LogsAndCallsDecoratedService()
    {
        // Arrange
        var mockAllMembersInDBService = new Mock<IAllMembersInDBService>();
        var mockDecoratedService = new Mock<IMembersListSelectionService>();
        var mockLogger = new Mock<ILoggerService>();
        var service = new LoggingMemberSelectionServiceDecorator(mockAllMembersInDBService.Object, mockDecoratedService.Object, mockLogger.Object);

        int testMemberID = 1;
        var memberEntities = new List<MemberEntity>();
        var memberName = "Test Member";
        mockAllMembersInDBService.Setup(s => s.MemberNameDictionary).Returns(new Dictionary<int, string> { { testMemberID, memberName } });

        // Act
        await service.ProcessSelectedMemberAsync(testMemberID, memberEntities);

        // Assert
        mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
        mockDecoratedService.Verify(ds => ds.ProcessSelectedMemberAsync(testMemberID, memberEntities), Times.Once);
    }

    [Fact]
    public async Task ProcessSelectedMemberAsync_LogsExceptionWhenThrown()
    {
        // Arrange
        var mockAllMembersInDBService = new Mock<IAllMembersInDBService>();
        var dict = mockAllMembersInDBService.Setup(s => s.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Test Member" } });
        var mockDecoratedService = new Mock<IMembersListSelectionService>();
        var mockLogger = new Mock<ILoggerService>();
        var service = new LoggingMemberSelectionServiceDecorator(mockAllMembersInDBService.Object, mockDecoratedService.Object, mockLogger.Object);

        int testMemberID = 1;
        var memberEntities = new List<MemberEntity>();
        var exception = new Exception("Test exception");
        mockDecoratedService.Setup(ds => ds.ProcessSelectedMemberAsync(testMemberID, memberEntities)).ThrowsAsync(exception);

        // Act
        await service.ProcessSelectedMemberAsync(testMemberID, memberEntities);

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}
