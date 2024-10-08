﻿using DataAccess.Models;
using DataAccess.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class AdminRoleFilterStrategyTests
{
    private readonly Mock<IRepositoryMember> _dataManagerMock = new();
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
        _dataManagerMock.Setup(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("role", "Admin")).ReturnsAsync(expectedMembers);

        // Act
        var result = await _adminRoleFilterStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _dataManagerMock.Verify(dm => dm.GetMembersByMetaKeyAndMetaValueAsync("role", "Admin"), Times.Once);
    }
}