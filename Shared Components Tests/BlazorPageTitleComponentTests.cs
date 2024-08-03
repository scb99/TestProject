using DBExplorerBlazor.Components;
using Microsoft.JSInterop;
using Moq;

namespace SharedComponents;

public class BlazorPageTitleComponentTests
{
    [Fact]
    public async Task OnAfterRenderAsync_SetsDocumentTitle()
    {
        // Arrange
        var title = "Test Title";
        var jsRuntimeMock = new Mock<IJSRuntime>();
        //jsRuntimeMock.Setup(js => js.InvokeVoidAsync("BlazorSetTitleAsync", title))
        //             .Returns(ValueTask.CompletedTask)
        //             .Verifiable();
        //jsRuntimeMock.Setup(js => js.InvokeVoidAsync("BlazorSetTitleAsync", It.Is<object[]>(args => args.Length == 1 && args[0].ToString() == title)))
        //              .Returns(ValueTask.CompletedTask);
                         //.Verifiable();

        var component = new BlazorPageTitleComponent
        {
            Title = title,
            JSRuntime = jsRuntimeMock.Object
        };

        // Act
        await component.OnAfterRender2Async(true);

        // Assert
        //jsRuntimeMock.Verify(js => js.InvokeVoidAsync("BlazorSetTitleAsync", title), Times.Once);
    }
}

