using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class LastNameFilterStrategyTests
{
    private readonly Mock<ICrossCuttingFilterUtility> mockFilterUtility;
    private readonly LastNameFilterStrategy lastNameFilterStrategy;

    public LastNameFilterStrategyTests()
    {
        mockFilterUtility = new Mock<ICrossCuttingFilterUtility>();
        lastNameFilterStrategy = new LastNameFilterStrategy(mockFilterUtility.Object);
    }

    [Fact]
    public void Filter_ReturnsFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { LastName = "Doe" },
            new MemberEntity { LastName = "Smith" },
            new MemberEntity { LastName = "Johnson" }
        };
        var filterValue = "Do";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((lastName, value) => lastName.Contains(value));

        // Act
        var result = lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal("Doe", result[0].LastName);
    }

    [Fact]
    public void Filter_ReturnsEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { LastName = "Doe" },
            new MemberEntity { LastName = "Smith" },
            new MemberEntity { LastName = "Johnson" }
        };
        var filterValue = "X";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((lastName, value) => lastName.Contains(value));

        // Act
        var result = lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ReturnsAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { LastName = "Doe" },
            new MemberEntity { LastName = "Smith" },
            new MemberEntity { LastName = "Johnson" }
        };
        var filterValue = "";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((lastName, value) => true);

        // Act
        var result = lastNameFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}