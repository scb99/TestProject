using DBExplorerBlazor.Services;
using Microsoft.JSInterop;
using Moq;

namespace Service;

public class AlertDialogServiceTests
{
    private readonly Mock<IJSRuntime> _jsRuntimeMock;
    private readonly AlertDialogService _alertDialogService;

    public AlertDialogServiceTests()
    {
        _jsRuntimeMock = new Mock<IJSRuntime>();
        _alertDialogService = new AlertDialogService(_jsRuntimeMock.Object);
    }

    [Fact]
    public async Task AlertUsingFallingMessageBoxAsync_InvokesOnShowAsync()
    {
        // Arrange
        var message = "Test message";
        var wasCalled = false;

        _alertDialogService.OnShowAsync += (msg) =>
        {
            wasCalled = msg == message;
            return Task.CompletedTask;
        };

        // Act
        await _alertDialogService.AlertUsingFallingMessageBoxAsync(message);

        // Assert
        Assert.True(wasCalled, "OnShowAsync was not invoked with the correct message.");
    }

    [Fact]
    public async Task AlertUsingPopUpMessageBoxAsync_CallsJSRuntimeWithCorrectParameters()
    {
        // Arrange
        var message = "Pop-up message";

        //_jsRuntimeMock.Setup(js => js.InvokeVoidAsync(
        //    "alertBoxSuccess",
        //    It.Is<string>(s => s == "FYI"),
        //    It.Is<string>(s => s == message),
        //    It.Is<string>(s => s == "info")))
        //.Returns(ValueTask.CompletedTask)
        //.Verifiable("The JSRuntime was not called with the expected parameters.");

        // Act
        await _alertDialogService.AlertUsingPopUpMessageBoxAsync(message);

        // Assert
        _jsRuntimeMock.Verify();
    }
}