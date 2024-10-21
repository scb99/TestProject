using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;

namespace CrossCuttingConcerns;

public class FilterUtilityTests
{
    private readonly ICrossCuttingFilterUtility filterUtility;

    public FilterUtilityTests()
    {
        filterUtility = new FilterUtility();
    }

    [Theory]
    [InlineData("JohnDoe", "John", true)]
    [InlineData("JaneDoe", "Jane", true)]
    [InlineData("JohnDoe", "Jane", false)]
    [InlineData("JohnDoe", "john", true)]
    [InlineData("JohnDoe", "JOHN", true)]
    [InlineData("JohnDoe", "", true)]
    [InlineData("JohnDoe", null, true)]
    [InlineData("JohnDoe", "Doe", false)]
    public void ShouldMemberBeIncluded_ShouldReturnExpectedResult(string nameOrID, string filter, bool expectedResult)
    {
        // Act
        var result = filterUtility.ShouldMemberBeIncluded(nameOrID, filter);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShouldMemberBeIncluded_ShouldThrowArgumentNullException_WhenNameOrIDIsNull()
    {
        // Arrange
        string? nameOrID = null;
        string filter = "test";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => filterUtility.ShouldMemberBeIncluded(nameOrID, filter));
    }
}
    