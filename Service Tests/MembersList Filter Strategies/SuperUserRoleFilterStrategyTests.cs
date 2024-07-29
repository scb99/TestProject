using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class SuperUserRoleFilterStrategyTests
{
    [Fact]
    public async Task FilterAsync_ReturnsSuperUserMembers()
    {
        // Arrange
        var mockDataManager = new Mock<IDataManager>();
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, Name = "SuperUser1" },
            new() { ID = 2, Name = "SuperUser2" }
        };

        mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "SuperUser"))
            .ReturnsAsync(expectedMembers);

        var strategy = new SuperUserRoleFilterStrategy(mockDataManager.Object);

        // Act
        var result = await strategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
    }

    [Fact]
    public async Task FilterAsync_ReturnsEmptyList_WhenNoSuperUserMembers()
    {
        // Arrange
        var mockDataManager = new Mock<IDataManager>();
        var expectedMembers = new List<MemberEntity>();

        mockDataManager
            .Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "SuperUser"))
            .ReturnsAsync(expectedMembers);

        var strategy = new SuperUserRoleFilterStrategy(mockDataManager.Object);

        // Act
        var result = await strategy.FilterAsync();

        // Assert
        Assert.Empty(result);
    }
}