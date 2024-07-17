using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class BoardMemberRoleFilterStrategyTests
{
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly BoardMemberRoleFilterStrategy _boardMemberRoleFilterStrategy;

    public BoardMemberRoleFilterStrategyTests() 
        => _boardMemberRoleFilterStrategy = new BoardMemberRoleFilterStrategy(_dataManagerMock.Object);

    [Fact]
    public async Task FilterAsync_ReturnsBoardMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _dataManagerMock.Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "BoardMember")).ReturnsAsync(expectedMembers);

        // Act
        var result = await _boardMemberRoleFilterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _dataManagerMock.Verify(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "BoardMember"), Times.Once);
    }
}