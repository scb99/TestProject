using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class AdminRoleFilterStrategyTests
{
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly AdminRoleFilterStrategy _adminRoleFilterStrategy;

    public AdminRoleFilterStrategyTests() 
        => _adminRoleFilterStrategy = new AdminRoleFilterStrategy(_dataManagerMock.Object);

    [Fact]
    public async Task FilterAsync_ReturnsAdminRoles()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _dataManagerMock.Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "Admin")).ReturnsAsync(expectedMembers);

        // Act
        var result = await _adminRoleFilterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _dataManagerMock.Verify(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("role", "Admin"), Times.Once);
    }
}