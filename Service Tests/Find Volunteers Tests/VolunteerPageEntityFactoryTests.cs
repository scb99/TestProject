using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace MenuItemComponents;

public class VolunteerPageEntityFactoryTests
{
    private readonly VolunteerPageEntityFactory _factory;

    public VolunteerPageEntityFactoryTests()
    {
        _factory = new VolunteerPageEntityFactory();
    }

    [Fact]
    public void CreateVolunteerEntity_InitializesPropertiesCorrectly()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity
        {
            Name = "John Doe",
            LastName = "Doe",
            FirstName = "John",
            Email = "john.doe@example.com",
            HomePhone = "123-456-7890"
        };

        // Act
        var result = _factory.CreateVolunteerEntity(volunteerEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(volunteerEntity, result.VolunteerEntity);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("john.doe@example.com", result.Email);
        Assert.Equal("123-456-7890", result.HomePhone);
    }
}