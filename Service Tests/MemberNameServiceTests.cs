using DBExplorerBlazor.Services;

namespace Service;

public class MemberNameServiceTests
{
    [Fact]
    public void MemberFirstName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new MemberNameService();
        var expectedFirstName = "John";

        // Act
        service.MemberFirstName = expectedFirstName;
        var actualFirstName = service.MemberFirstName;

        // Assert
        Assert.Equal(expectedFirstName, actualFirstName);
    }

    [Fact]
    public void MemberLastName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new MemberNameService();
        var expectedLastName = "Doe";

        // Act
        service.MemberLastName = expectedLastName;
        var actualLastName = service.MemberLastName;

        // Assert
        Assert.Equal(expectedLastName, actualLastName);
    }

    [Fact]
    public void MemberName_InitializedWithDefaultValue()
    {
        // Arrange
        var service = new MemberNameService();
        var expectedDefaultValue = "<No Member Selected>";

        // Act
        var actualValue = service.MemberName;

        // Assert
        Assert.Equal(expectedDefaultValue, actualValue);
    }

    [Fact]
    public void MemberName_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var service = new MemberNameService();
        var expectedName = "John Doe";

        // Act
        service.MemberName = expectedName;
        var actualName = service.MemberName;

        // Assert
        Assert.Equal(expectedName, actualName);
    }
}