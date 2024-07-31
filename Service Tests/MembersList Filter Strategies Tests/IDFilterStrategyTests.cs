using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersListFilterStrategy;

public class IDFilterStrategyTests
{
    private readonly Mock<ICrossCuttingFilterUtility> mockFilterUtility;
    private readonly IDFilterStrategy idFilterStrategy;

    public IDFilterStrategyTests()
    {
        mockFilterUtility = new Mock<ICrossCuttingFilterUtility>();
        idFilterStrategy = new IDFilterStrategy(mockFilterUtility.Object);
    }

    [Fact]
    public void Filter_ReturnsFilteredMembers_WhenFilterValueMatches()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { ID = 1 },
            new MemberEntity { ID = 2 },
            new MemberEntity { ID = 3 }
        };
        var filterValue = "1";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((id, value) => id.Contains(value));

        // Act
        var result = idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].ID);
    }

    [Fact]
    public void Filter_ReturnsEmptyList_WhenNoMembersMatchFilterValue()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { ID = 1 },
            new MemberEntity { ID = 2 },
            new MemberEntity { ID = 3 }
        };
        var filterValue = "4";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((id, value) => id.Contains(value));

        // Act
        var result = idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Filter_ReturnsAllMembers_WhenFilterValueIsEmpty()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { ID = 1 },
            new MemberEntity { ID = 2 },
            new MemberEntity { ID = 3 }
        };
        var filterValue = "";

        mockFilterUtility.Setup(util => util.ShouldMemberBeIncluded(It.IsAny<string>(), filterValue))
            .Returns<string, string>((id, value) => true);

        // Act
        var result = idFilterStrategy.Filter(members, filterValue);

        // Assert
        Assert.Equal(3, result.Count);
    }
}