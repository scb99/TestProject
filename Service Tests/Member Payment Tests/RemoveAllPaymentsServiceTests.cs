using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberPayment;

public class RemoveAllPaymentsServiceTests
{
    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly RemoveAllPaymentsService _removeAllPaymentsService;

    public RemoveAllPaymentsServiceTests()
    {
        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
        _loggerMock = new Mock<ILoggerService>();
        _removeAllPaymentsService = new RemoveAllPaymentsService(_paymentsServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RemoveAllPaymentsAsync_CallsRemoveAllPaymentsFromPaymentEntities()
    {
        // Act
        await _removeAllPaymentsService.RemoveAllPaymentsAsync();

        // Assert
        _paymentsServiceMock.Verify(p => p.RemoveAllPaymentsFromPaymentEntities(), Times.Once);
    }

    [Fact]
    public async Task RemoveAllPaymentsAsync_LogsException_WhenExceptionIsThrown()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _paymentsServiceMock.Setup(p => p.RemoveAllPaymentsFromPaymentEntities()).Throws(exception);

        // Act
        await _removeAllPaymentsService.RemoveAllPaymentsAsync();

        // Assert
        _loggerMock.Verify(l => l.LogExceptionAsync(exception, nameof(RemoveAllPaymentsService)), Times.Once);
    }
}