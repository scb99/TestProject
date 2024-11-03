using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace MenuItemComponents;

public class VolunteerPageKeyGeneratorServiceTests
{
    private readonly VolunteerPageKeyGeneratorService _service;

    public VolunteerPageKeyGeneratorServiceTests()
    {
        _service = new VolunteerPageKeyGeneratorService();
    }

    [Fact]
    public void GenerateKey_ReturnsCorrectKey()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity
        {
            Name = "John Doe",
            ID = "123"
        };

        // Act
        var result = _service.GenerateKey(volunteerEntity);

        // Assert
        Assert.Equal("John Doe/123", result);
    }

    [Fact]
    public void GenerateKey_ReturnsCorrectKey_WithDifferentValues()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity
        {
            Name = "Jane Smith",
            ID = "456"
        };

        // Act
        var result = _service.GenerateKey(volunteerEntity);

        // Assert
        Assert.Equal("Jane Smith/456", result);
    }

    [Fact]
    public void GenerateKey_ReturnsCorrectKey_WithEmptyName()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity
        {
            Name = "",
            ID = "789"
        };

        // Act
        var result = _service.GenerateKey(volunteerEntity);

        // Assert
        Assert.Equal("/789", result);
    }

    [Fact]
    public void GenerateKey_ReturnsCorrectKey_WithNullName()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity
        {
            Name = null,
            ID = "101"
        };

        // Act
        var result = _service.GenerateKey(volunteerEntity);

        // Assert
        Assert.Equal("/101", result);
    }
}