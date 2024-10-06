using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class DeceasedMembersFilterStrategyTests
{
    private readonly Mock<IRepositoryMember> _mockDataManager = new();
    private readonly IDeceasedMembersFilterStrategy _filterStrategy;

    public DeceasedMembersFilterStrategyTests() 
        => _filterStrategy = new DeceasedMembersFilterStrategy(_mockDataManager.Object);

    [Fact]
    public async Task FilterAsync_ReturnsDeceasedMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" },
            new() { ID = 2, FirstName = "Jane", LastName = "Doe" }
        };

        _mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("deceased", "Yes"))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _filterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
    }

    [Fact]
    public async Task FilterAsync_ReturnsEmptyList_WhenNoDeceasedMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>();

        _mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("deceased", "Yes"))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _filterStrategy.FilterAsync();

        // Assert
        Assert.Empty(result);
    }
}