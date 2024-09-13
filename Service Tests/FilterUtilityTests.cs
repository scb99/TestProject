//using DBExplorerBlazor.Services;

//namespace Service;

//public class FilterUtilityTests
//{
//    [Theory]
//    [InlineData("JohnDoe", "John", true)]
//    [InlineData("JaneDoe", "Jane", true)]
//    [InlineData("JohnDoe", "Jane", false)]
//    [InlineData("JohnDoe", "john", true)]
//    [InlineData("JohnDoe", "JOHN", true)]
//    [InlineData("JohnDoe", "", true)]
//    [InlineData("JohnDoe", null, true)]
//    [InlineData("JohnDoe", "Doe", false)]
//    public void ShouldMemberBeIncluded_ShouldReturnExpectedResult(string nameOrID, string filter, bool expectedResult)
//    {
//        // Act
//        var result = FilterUtility.ShouldMemberBeIncluded(nameOrID, filter);

//        // Assert
//        Assert.Equal(expectedResult, result);
//    }

//    [Fact]
//    public void ShouldMemberBeIncluded_ShouldThrowArgumentNullException_WhenNameOrIDIsNull()
//    {
//        // Arrange
//        string? nameOrID = null;
//        string filter = "test";

//        // Act & Assert
//        Assert.Throws<ArgumentNullException>(() => FilterUtility.ShouldMemberBeIncluded(nameOrID, filter));
//    }
//}
    