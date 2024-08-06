using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersList;

public class LoggingMembersLoadServiceDecoratorTests
{
    [Fact]
    public async Task LoadMembersAsync_ReturnsMembersOnSuccess()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockInnerService = new Mock<IRetrieveMembersListDataService>();
        var expectedMembers = new List<MemberEntity> { new() };
        mockInnerService.Setup(s => s.RetrieveMembersAsync()).ReturnsAsync(expectedMembers);
        var decorator = new LoggingMembersLoadServiceDecorator(mockLogger.Object, mockInnerService.Object);

        // Act
        var result = await decorator.RetrieveMembersAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoadMembersAsync_LogsExceptionAndReturnsEmptyListOnFailure()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockInnerService = new Mock<IRetrieveMembersListDataService>();
        var exception = new Exception("Test exception");
        mockInnerService.Setup(s => s.RetrieveMembersAsync()).ThrowsAsync(exception);
        var decorator = new LoggingMembersLoadServiceDecorator(mockLogger.Object, mockInnerService.Object);

        // Act
        var result = await decorator.RetrieveMembersAsync();

        // Assert
        Assert.Empty(result);
        mockLogger.Verify(l => l.LogExceptionAsync(exception, "Error loading members"), Times.Once);
    }
}