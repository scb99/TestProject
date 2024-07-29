using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace MembersListFilterStrategy;

public class LastNameFilterStrategyTests
{
    private readonly LastNameFilterStrategy _lastNameFilterStrategy;

    public LastNameFilterStrategyTests() 
        => _lastNameFilterStrategy = new LastNameFilterStrategy();

    [Fact]
    public void Filter_ShouldReturnFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { LastName = "Smith" },
            new() { LastName = "Johnson" },
            new() { LastName = "Williams" }
        };
        var filterValue = "Smi";

        // Act
        var result = _lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal("Smith", result[0].LastName);
    }

    [Fact]
    public void Filter_ShouldReturnEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { LastName = "Smith" },
            new() { LastName = "Johnson" },
            new() { LastName = "Williams" }
        };
        var filterValue = "XYZ";

        // Act
        var result = _lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { LastName = "Smith" },
            new() { LastName = "Johnson" },
            new() { LastName = "Williams" }
        };
        var filterValue = "";

        // Act
        var result = _lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsNull()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { LastName = "Smith" },
            new() { LastName = "Johnson" },
            new() { LastName = "Williams" }
        };
        string? filterValue = null;

        // Act
        var result = _lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}