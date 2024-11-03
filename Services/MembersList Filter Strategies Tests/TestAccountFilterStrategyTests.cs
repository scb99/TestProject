using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class TestAccountFilterStrategyTests
{
    [Fact]
    public async Task FilterAsync_ReturnsTestAccountMembers()
    {
        // Arrange
        var mockDataManager = new Mock<IRepositoryMember>();
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, Name = "TestUser1" },
            new() { ID = 2, Name = "TestUser2" }
        };

        mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("test_account", "Yes"))
            .ReturnsAsync(expectedMembers);

        var strategy = new TestAccountFilterStrategy(mockDataManager.Object);

        // Act
        var result = await strategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
    }

    [Fact]
    public async Task FilterAsync_ReturnsEmptyList_WhenNoTestAccountMembers()
    {
        // Arrange
        var mockDataManager = new Mock<IRepositoryMember>();
        var expectedMembers = new List<MemberEntity>();

        mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("test_account", "Yes"))
            .ReturnsAsync(expectedMembers);

        var strategy = new TestAccountFilterStrategy(mockDataManager.Object);

        // Act
        var result = await strategy.FilterAsync();

        // Assert
        Assert.Empty(result);
    }
}