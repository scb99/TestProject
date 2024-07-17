using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class ComboBoxProcessServiceTests
{
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<IMemberProcessingService> _memberProcessingServiceMock;
    private readonly ComboBoxProcessService _comboBoxProcessService;

    public ComboBoxProcessServiceTests()
    {
        _loggerMock = new Mock<ILoggerService>();
        _memberProcessingServiceMock = new Mock<IMemberProcessingService>();
        _comboBoxProcessService = new ComboBoxProcessService(_loggerMock.Object, _memberProcessingServiceMock.Object);
    }

    [Fact]
    public async Task ProcessComboBoxChangeAsync_ReturnsMembers_OnSuccess()
    {
        // Arrange
        string selection = "TestSelection";
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _memberProcessingServiceMock.Setup(m => m.ProcessComboBoxChangeAsync(selection)).ReturnsAsync(expectedMembers);

        // Act
        var result = await _comboBoxProcessService.ProcessComboBoxChangeAsync(selection);

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _memberProcessingServiceMock.Verify(m => m.ProcessComboBoxChangeAsync(selection), Times.Once);
    }

    [Fact]
    public async Task ProcessComboBoxChangeAsync_LogsException_OnFailure()
    {
        // Arrange
        string selection = "TestSelection";
        var exception = new Exception("Test exception");
        _memberProcessingServiceMock.Setup(m => m.ProcessComboBoxChangeAsync(selection)).ThrowsAsync(exception);

        // Act
        var result = await _comboBoxProcessService.ProcessComboBoxChangeAsync(selection);

        // Assert
        Assert.Empty(result);
        _loggerMock.Verify(l => l.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}