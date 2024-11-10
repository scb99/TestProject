namespace ExtensionMethods;

public class ObjectExtensionsTests
{
    private class TestClass
    {
        public int Value { get; set; }
        public string? Text { get; set; }
        public List<int>? Numbers { get; set; }
    }

    [Fact]
    public void Copy_CreatesDeepCopyOfObject()
    {
        // Arrange
        var original = new TestClass
        {
            Value = 42,
            Text = "Hello",
            Numbers = new List<int> { 1, 2, 3 }
        };

        // Act
        var copy = original.Copy();

        // Assert
        Assert.NotSame(original, copy);
        Assert.Equal(original.Value, copy.Value);
        Assert.Equal(original.Text, copy.Text);
        Assert.NotSame(original.Numbers, copy.Numbers);
        Assert.Equal(original.Numbers, copy.Numbers);
    }

    [Fact]
    public void IsPrimitive_ReturnsTrueForPrimitiveTypes()
    {
        // Arrange
        var intType = typeof(int);
        var stringType = typeof(string);

        // Act
        var isIntPrimitive = intType.IsPrimitive();
        var isStringPrimitive = stringType.IsPrimitive();

        // Assert
        Assert.True(isIntPrimitive);
        Assert.True(isStringPrimitive);
    }

    [Fact]
    public void IsPrimitive_ReturnsFalseForNonPrimitiveTypes()
    {
        // Arrange
        var listType = typeof(List<int>);

        // Act
        var isListPrimitive = listType.IsPrimitive();

        // Assert
        Assert.False(isListPrimitive);
    }

    //[Fact]
    //public void IsMock_ReturnsTrueForProxyObjects()
    //{
    //    // Arrange
    //    var proxyObject = new { Name = "Proxy" };

    //    // Act
    //    var isMock = proxyObject.IsMock();

    //    // Assert
    //    Assert.True(isMock);
    //}

    [Fact]
    public void IsMock_ReturnsFalseForNonProxyObjects()
    {
        // Arrange
        var normalObject = new { Name = "Normal" };

        // Act
        var isMock = normalObject.IsMock();

        // Assert
        Assert.False(isMock);
    }

    [Fact]
    public void GetClassDotMethodName_ReturnsCorrectMethodName()
    {
        // Arrange
        var obj = new object();

        // Act
        var methodName = obj.GetClassDotMethodName();

        // Assert
        Assert.Equal("ObjectExtensionsTests.GetClassDotMethodName_ReturnsCorrectMethodName", methodName);
    }
}