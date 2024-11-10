using System.Reflection;
using System.Runtime.CompilerServices;
using DBExplorerBlazor;

namespace ExtensionMethods;

public class MethodBaseExtensionsTests
{
    private class TestClass
    {
        public static void TestMethod() { }

        public static async Task TestAsyncMethod()
        {
            await Task.Delay(1);
        }
    }

    [Fact]
    public void GetMethodContextName_ReturnsCorrectName_ForNonAsyncMethod()
    {
        // Arrange
        var method = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod));

        // Act
        var result = method.GetMethodContextName();

        // Assert
        Assert.Equal("TestClass.TestMethod", result);
    }

    [Fact]
    public void GetMethodContextName_ReturnsCorrectName_ForAsyncMethod()
    {
        // Arrange
        var method = typeof(TestClass).GetMethod(nameof(TestClass.TestAsyncMethod))?.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.NonPublic);

        // Act
        var result = method.GetMethodContextName();

        // Assert
        Assert.Equal("TestClass.TestAsyncMethod", result);
    }
}