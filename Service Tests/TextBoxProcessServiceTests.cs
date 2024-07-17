using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class TextBoxProcessServiceTests
{
    private readonly Mock<ILoggerService> _loggerServiceMock = new();
    private readonly Mock<IMemberFilteringService> _memberFilteringServiceMock = new();
    private readonly TextBoxProcessService _textBoxProcessService;

    public TextBoxProcessServiceTests()
    {
        _textBoxProcessService = new TextBoxProcessService(_loggerServiceMock.Object, _memberFilteringServiceMock.Object);
    }

    [Fact]
    public async Task ProcessTextBoxInputChangeAsync_ReturnsFilteredMembers_OnSuccess()
    {
        // Arrange
        var inputValue = "test";
        var filterCriteria = "name";
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _memberFilteringServiceMock.Setup(service => service.FilterMembersAsync(inputValue, filterCriteria)).ReturnsAsync(expectedMembers);

        // Act
        var result = await _textBoxProcessService.ProcessTextBoxInputChangeAsync(inputValue, filterCriteria);

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
    }

    [Fact]
    public async Task ProcessTextBoxInputChangeAsync_ReturnsEmptyList_OnException()
    {
        // Arrange
        var inputValue = "test";
        var filterCriteria = "name";
        _memberFilteringServiceMock.Setup(service => service.FilterMembersAsync(inputValue, filterCriteria)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _textBoxProcessService.ProcessTextBoxInputChangeAsync(inputValue, filterCriteria);

        // Assert
        Assert.Empty(result);
        _loggerServiceMock.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}

