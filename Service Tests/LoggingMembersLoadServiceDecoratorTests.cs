using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class LoggingMembersLoadServiceDecoratorTests
{
    [Fact]
    public async Task LoadMembersAsync_ReturnsMembersOnSuccess()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockInnerService = new Mock<IMembersListLoadService>();
        var expectedMembers = new List<MemberEntity> { new() };
        mockInnerService.Setup(s => s.LoadMembersAsync()).ReturnsAsync(expectedMembers);
        var decorator = new LoggingMembersLoadServiceDecorator(mockLogger.Object, mockInnerService.Object);

        // Act
        var result = await decorator.LoadMembersAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoadMembersAsync_LogsExceptionAndReturnsEmptyListOnFailure()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockInnerService = new Mock<IMembersListLoadService>();
        var exception = new Exception("Test exception");
        mockInnerService.Setup(s => s.LoadMembersAsync()).ThrowsAsync(exception);
        var decorator = new LoggingMembersLoadServiceDecorator(mockLogger.Object, mockInnerService.Object);

        // Act
        var result = await decorator.LoadMembersAsync();

        // Assert
        Assert.Empty(result);
        mockLogger.Verify(l => l.LogExceptionAsync(exception, "Error loading members"), Times.Once);
    }
}