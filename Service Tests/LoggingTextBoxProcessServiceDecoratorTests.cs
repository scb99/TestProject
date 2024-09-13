using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class LoggingTextBoxProcessServiceDecoratorTests
{
    [Fact]
    public async Task ProcessTextBoxInputChangeAsync_CallsDecoratedService()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockDecoratedService = new Mock<IMembersListTextBoxProcessingService>();
        var decorator = new LoggingTextBoxProcessServiceDecorator(mockLogger.Object, mockDecoratedService.Object);
        var testValue = "test";
        var testFilterCriteria = "criteria";
        var expectedMembers = new List<MemberEntity>();

        mockDecoratedService.Setup(service => service.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await decorator.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria);

        // Assert
        mockDecoratedService.Verify(service => service.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria), Times.Once);
        Assert.Equal(expectedMembers, result);
    }

    [Fact]
    public async Task ProcessTextBoxInputChangeAsync_LogsExceptionOnFailure()
    {
        // Arrange
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockDecoratedService = new Mock<IMembersListTextBoxProcessingService>();
        var decorator = new LoggingTextBoxProcessServiceDecorator(mockLogger.Object, mockDecoratedService.Object);
        var testValue = "test";
        var testFilterCriteria = "criteria";
        var exception = new Exception("Test exception");

        mockDecoratedService.Setup(service => service.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria))
            .ThrowsAsync(exception);

        // Act
        var result = await decorator.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria);

        // Assert
        mockLogger.Verify(logger => logger.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        Assert.Empty(result); // Ensure the result is an empty list as per the catch block logic.
    }
}
