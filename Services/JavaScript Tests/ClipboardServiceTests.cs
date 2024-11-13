using DBExplorerBlazor.Services;
using Microsoft.JSInterop;
using Moq;

namespace JavaScript;

public class ClipboardServiceTests
{
    private readonly Mock<IJSRuntime> _jsRuntimeMock;
    private readonly ClipboardService _clipboardService;

    public ClipboardServiceTests()
    {
        _jsRuntimeMock = new Mock<IJSRuntime>();
        _clipboardService = new ClipboardService(_jsRuntimeMock.Object);
    }

    [Fact]
    public async Task CopyToClipboardAsync_CallsJSRuntime()
    {
        // Arrange
        string textToCopy = "Test text";
        _jsRuntimeMock.Setup(js => js.InvokeAsync<object>(
            "navigator.clipboard.writeText",
            It.IsAny<object[]>()))
         .ReturnsAsync(new object()); // Assume success if this returns without exception

        // Act and Assert
        await _clipboardService.CopyToClipboardAsync(textToCopy);
    }
}