using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class LifetimeMembersFilterStrategyTests
{
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly LifetimeMembersFilterStrategy _lifetimeMembersFilterStrategy;

    public LifetimeMembersFilterStrategyTests() 
        => _lifetimeMembersFilterStrategy = new LifetimeMembersFilterStrategy(_dataManagerMock.Object);

    [Fact]
    public async Task FilterAsync_ShouldReturnLifetimeMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" },
            new() { ID = 2, FirstName = "Jane", LastName = "Smith" }
        };

        _dataManagerMock
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("lifetime_member", "Yes"))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _lifetimeMembersFilterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        Assert.Equal(expectedMembers[0].ID, result[0].ID);
        Assert.Equal(expectedMembers[1].ID, result[1].ID);
    }

    [Fact]
    public async Task FilterAsync_ShouldReturnEmptyList_WhenNoLifetimeMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>();

        _dataManagerMock
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("lifetime_member", "Yes"))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _lifetimeMembersFilterStrategy.FilterAsync();

        // Assert
        Assert.Empty(result);
    }
}