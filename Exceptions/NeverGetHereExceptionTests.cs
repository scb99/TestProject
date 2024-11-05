using DBExplorerBlazor;

namespace Exceptions;

public class NeverGetHereExceptionTests
{
    [Fact]
    public void NeverGetHereException_ShouldInheritFromException()
    {
        // Arrange & Act
        var exception = new NeverGetHereException("Test message");

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void NeverGetHereException_ShouldSetMessageProperty()
    {
        // Arrange
        var testMessage = "Test message";

        // Act
        var exception = new NeverGetHereException(testMessage);

        // Assert
        Assert.Equal(testMessage, exception.Message);
    }

    [Fact]
    public void NeverGetHereException_ShouldBeSerializable()
    {
        // Arrange
        var exception = new NeverGetHereException("Test message");

        // Act
        var isSerializable = exception.GetType().IsSerializable;

        // Assert
        Assert.True(isSerializable);
    }
}