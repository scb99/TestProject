using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;

namespace Service;

public class LoggingPaymentActionHandlerDecoratorTests
{
    [Fact]
    public async Task HandleActionAsync_Calls_InnerHandler()
    {
        // Arrange
        var mockLogger = new Mock<ILoggerService>();
        var mockInnerHandler = new Mock<IPaymentActionHandler>();
        var decorator = new LoggingPaymentActionHandlerDecorator(mockLogger.Object, mockInnerHandler.Object);
        var args = new ActionEventArgs<PaymentEntity>();
        var grid = new SfGrid<PaymentEntity>();
        string amount = "100";

        // Act
        await decorator.HandleActionAsync(args, grid, amount);

        // Assert
        mockInnerHandler.Verify(i => i.HandleActionAsync(args, grid, amount), Times.Once);
    }

    [Fact]
    public async Task HandleActionAsync_Logs_Exceptions()
    {
        // Arrange
        var mockLogger = new Mock<ILoggerService>();
        var mockInnerHandler = new Mock<IPaymentActionHandler>();
        mockInnerHandler.Setup(i => i.HandleActionAsync(It.IsAny<ActionEventArgs<PaymentEntity>>(), It.IsAny<SfGrid<PaymentEntity>>(), It.IsAny<string>()))
                        .ThrowsAsync(new Exception("Test exception"));
        var decorator = new LoggingPaymentActionHandlerDecorator(mockLogger.Object, mockInnerHandler.Object);
        var args = new ActionEventArgs<PaymentEntity>();
        var grid = new SfGrid<PaymentEntity>();
        string amount = "100";

        // Act
        await decorator.HandleActionAsync(args, grid, amount);

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void OnValueChange_Calls_InnerHandler()
    {
        // Arrange
        var mockLogger = new Mock<ILoggerService>();
        var mockInnerHandler = new Mock<IPaymentActionHandler>();
        var decorator = new LoggingPaymentActionHandlerDecorator(mockLogger.Object, mockInnerHandler.Object);
        var args = new ChangeEventArgs<string, DropDownItems>();
        string amount = "100";

        // Act
        decorator.OnValueChange(args, ref amount);

        // Assert
        mockInnerHandler.Verify(i => i.OnValueChange(args, ref amount), Times.Once);
    }

    [Fact]
    public void OnValueChange_Logs_Exceptions()
    {
        // Arrange
        var mockLogger = new Mock<ILoggerService>();
        var mockInnerHandler = new Mock<IPaymentActionHandler>();
        mockInnerHandler.Setup(i => i.OnValueChange(It.IsAny<ChangeEventArgs<string, DropDownItems>>(), ref It.Ref<string>.IsAny))
                        .Throws(new Exception("Test exception"));
        var decorator = new LoggingPaymentActionHandlerDecorator(mockLogger.Object, mockInnerHandler.Object);
        var args = new ChangeEventArgs<string, DropDownItems>();
        string amount = "100";

        // Act
        try
        {
            decorator.OnValueChange(args, ref amount);
        }
        catch
        {
            // Exception is expected
        }

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}