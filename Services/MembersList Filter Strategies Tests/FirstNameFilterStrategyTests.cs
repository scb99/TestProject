using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class FirstNameFilterStrategyTests
{
    private readonly Mock<ICrossCuttingFilterUtility> mockFilterUtility;
    private readonly FirstNameFilterStrategy firstNameFilterStrategy;

    public FirstNameFilterStrategyTests()
    {
        mockFilterUtility = new Mock<ICrossCuttingFilterUtility>();
        firstNameFilterStrategy = new FirstNameFilterStrategy(mockFilterUtility.Object);
    }

    [Fact]
    public void Filter_ReturnsFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { FirstName = "John" },
            new MemberEntity { FirstName = "Jane" },
            new MemberEntity { FirstName = "Doe" }
        };
        var filterValue = "Jo";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((firstName, value) => firstName.Contains(value));

        // Act
        var result = firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
    }

    [Fact]
    public void Filter_ReturnsEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { FirstName = "John" },
            new MemberEntity { FirstName = "Jane" },
            new MemberEntity { FirstName = "Doe" }
        };
        var filterValue = "X";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((firstName, value) => firstName.Contains(value));

        // Act
        var result = firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ReturnsAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { FirstName = "John" },
            new MemberEntity { FirstName = "Jane" },
            new MemberEntity { FirstName = "Doe" }
        };
        var filterValue = "";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((firstName, value) => true);

        // Act
        var result = firstNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}