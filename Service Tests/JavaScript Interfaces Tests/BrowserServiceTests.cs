using DBExplorerBlazor.Services;
using Microsoft.JSInterop;
using Moq;

namespace JavaScript;

public class BrowserServiceTests
{
    private readonly Mock<IJSRuntime> _jsRuntimeMock;
    private readonly BrowserService _browserService;

    public BrowserServiceTests()
    {
        _jsRuntimeMock = new Mock<IJSRuntime>();
        _browserService = new BrowserService(_jsRuntimeMock.Object);
    }

    [Fact]
    public async Task GetDimensionsAsync_ReturnsCorrectDimensions()
    {
        // Arrange
        var expectedDimensions = new BrowserDimension { Width = 1920, Height = 1080 };
        _jsRuntimeMock.Setup(js => js.InvokeAsync<BrowserDimension>(
            "getDimensions",
            It.IsAny<object[]>()))
            .ReturnsAsync(expectedDimensions);

        // Act
        var result = await _browserService.GetDimensionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDimensions.Width, result.Width);
        Assert.Equal(expectedDimensions.Height, result.Height);
        _jsRuntimeMock.Verify(js => js.InvokeAsync<BrowserDimension>("getDimensions", It.IsAny<object[]>()), Times.Once);
    }
}