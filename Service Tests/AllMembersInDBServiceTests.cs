﻿using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class AllMembersInDBServiceTests
{
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly AllMembersInDBService _allMembersInDBService;

    public AllMembersInDBServiceTests() 
        => _allMembersInDBService = new AllMembersInDBService(_dataManagerMock.Object);

    [Fact]
    public async Task GetAllMembersInDBAsync_ReturnsMembers_WhenCacheIsNull()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, Name = "John Doe" },
            new() { ID = 2, Name = "Jane Doe" }
        };
        _dataManagerMock.Setup(dm => dm.GetAllMembersSPAsync()).ReturnsAsync(expectedMembers);

        // Act
        var result = await _allMembersInDBService.GetAllMembersInDBAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        Assert.Equal(expectedMembers, result);
        _dataManagerMock.Verify(dm => dm.GetAllMembersSPAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMembersInDBAsync_ReturnsCachedMembers_WhenCacheIsNotNull()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { ID = 1, Name = "John Doe" },
            new() { ID = 2, Name = "Jane Doe" }
        };
        // Populate the cache first
        _allMembersInDBService.AllMembersInDB = expectedMembers;

        // Act
        var result = await _allMembersInDBService.GetAllMembersInDBAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        Assert.Equal(expectedMembers, result);
        _dataManagerMock.Verify(dm => dm.GetAllMembersSPAsync(), Times.Never); // Verify that the database is not hit again
    }

    [Fact]
    public void MemberNameDictionary_IsPopulatedCorrectly()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { ID = 1, Name = "John Doe" },
            new() { ID = 2, Name = "Jane Doe" }
        };
        _allMembersInDBService.AllMembersInDB = members;

        // Act
        var dictionary = _allMembersInDBService.MemberNameDictionary;

        // Assert
        Assert.Equal(members.Count, dictionary.Count);
        foreach (var member in members)
        {
            Assert.True(dictionary.ContainsKey(member.ID));
            Assert.Equal(member.Name, dictionary[member.ID]);
        }
    }
}