using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3.Services.MembersList;
using Moq;

namespace Service;

public class MembersListComboBoxProcessServiceTests
{
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<IMembersListComboBoxProcessingService> _memberProcessingServiceMock;
    private readonly MembersListComboBoxProcessService _comboBoxProcessService;

    public MembersListComboBoxProcessServiceTests()
    {
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _memberProcessingServiceMock = new Mock<IMembersListComboBoxProcessingService>();
        _comboBoxProcessService = new MembersListComboBoxProcessService(_loggerMock.Object, _memberProcessingServiceMock.Object);
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