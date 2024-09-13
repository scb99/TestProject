using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace Service;

public class MemberDetailsServiceTests
{
    [Fact]
    public void MemberDetailEntities_PropertyGetterSetter_ReturnsCorrectly()
    {
        // Arrange
        var service = new MemberDetailsService();
        var expectedList = new List<MemberDetailEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };

        // Act
        service.MemberDetailEntities = expectedList;
        var actualList = service.MemberDetailEntities;

        // Assert
        Assert.Equal(expectedList, actualList);
    }
}