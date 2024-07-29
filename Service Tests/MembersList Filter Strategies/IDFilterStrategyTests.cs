using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace MembersListFilterStrategy;

public class IDFilterStrategyTests
{
    private readonly IDFilterStrategy _idFilterStrategy;

    public IDFilterStrategyTests() 
        => _idFilterStrategy = new IDFilterStrategy();

    [Fact]
    public void Filter_ShouldReturnFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { ID = 123 },
            new() { ID = 456 },
            new() { ID = 789 }
        };
        var filterValue = "12";

        // Act
        var result = _idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal(123, result[0].ID);
    }

    [Fact]
    public void Filter_ShouldReturnEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { ID = 123 },
            new() { ID = 456 },
            new() { ID = 789 }
        };
        var filterValue = "999";

        // Act
        var result = _idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { ID = 123 },
            new() { ID = 456 },
            new() { ID = 789 }
        };
        var filterValue = "";

        // Act
        var result = _idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsNull()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { ID = 123 },
            new() { ID = 456 },
            new() { ID = 789 }
        };
        string? filterValue = null;

        // Act
        var result = _idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}
