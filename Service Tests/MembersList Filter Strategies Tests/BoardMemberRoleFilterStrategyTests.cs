using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class BoardMemberRoleFilterStrategyTests
{
    private readonly Mock<IRepositoryMember> _dataManagerMock = new();
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
        _dataManagerMock.Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("role", "BoardMember")).ReturnsAsync(expectedMembers);

        // Act
        var result = await _boardMemberRoleFilterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _dataManagerMock.Verify(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("role", "BoardMember"), Times.Once);
    }
}