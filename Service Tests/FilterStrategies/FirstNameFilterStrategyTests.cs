using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace Service;

public class FirstNameFilterStrategyTests
{
    private readonly FirstNameFilterStrategy _firstNameFilterStrategy;

    public FirstNameFilterStrategyTests() 
        => _firstNameFilterStrategy = new FirstNameFilterStrategy();

    [Fact]
    public void Filter_ShouldReturnFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { FirstName = "John" },
            new() { FirstName = "Jane" },
            new() { FirstName = "Doe" }
        };
        var filterValue = "Jo";

        // Act
        var result = _firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
    }

    [Fact]
    public void Filter_ShouldReturnEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { FirstName = "John" },
            new() { FirstName = "Jane" },
            new() { FirstName = "Doe" }
        };
        var filterValue = "XYZ";

        // Act
        var result = _firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { FirstName = "John" },
            new() { FirstName = "Jane" },
            new() { FirstName = "Doe" }
        };
        var filterValue = "";

        // Act
        var result = _firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Filter_ShouldReturnAllMembers_WhenFilterValueIsNull()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new() { FirstName = "John" },
            new() { FirstName = "Jane" },
            new() { FirstName = "Doe" }
        };
        string? filterValue = null;

        // Act
        var result = _firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}