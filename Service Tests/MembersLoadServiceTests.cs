using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MembersLoadServiceTests
{
    private readonly Mock<IAllMembersInDBService> _allMembersInDBServiceMock = new();
    private readonly Mock<ILoadingPanelService> _loadingPanelServiceMock = new();
    private readonly Mock<ILoggerService> _loggerServiceMock = new();
    private readonly MembersLoadService _membersLoadService;

    public MembersLoadServiceTests()
    {
        _membersLoadService = new MembersLoadService(_allMembersInDBServiceMock.Object, _loadingPanelServiceMock.Object, _loggerServiceMock.Object);
    }

    [Fact]
    public async Task LoadMembersAsync_ReturnsMembers_OnSuccess()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _allMembersInDBServiceMock.Setup(service => service.GetAllMembersInDBAsync()).ReturnsAsync(expectedMembers);

        // Act
        var result = await _membersLoadService.LoadMembersAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _loadingPanelServiceMock.Verify(lp => lp.ShowLoadingPanelAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadMembersAsync_ReturnsEmptyList_OnException()
    {
        // Arrange
        _allMembersInDBServiceMock.Setup(service => service.GetAllMembersInDBAsync()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _membersLoadService.LoadMembersAsync();

        // Assert
        Assert.Empty(result);
        _loggerServiceMock.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        _loadingPanelServiceMock.Verify(lp => lp.ShowLoadingPanelAsync(), Times.Once);
    }
}
