using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberPaymentServiceTests
{
    private readonly Mock<IPaymentsService> _paymentsServiceMock;
    private readonly Mock<IMemberNameService> _memberNameServiceMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MemberPaymentService _memberPaymentService;

    public MemberPaymentServiceTests()
    {
        _paymentsServiceMock = new Mock<IPaymentsService>();
        _memberNameServiceMock = new Mock<IMemberNameService>();
        _loggerMock = new Mock<ILoggerService>();

        _memberPaymentService = new MemberPaymentService(
            _paymentsServiceMock.Object,
            _memberNameServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task AddPaymentAsync_ShouldAddPayment()
    {
        // Arrange
        int selectedID = 1;
        _memberNameServiceMock.Setup(service => service.MemberFirstName).Returns("John");
        _memberNameServiceMock.Setup(service => service.MemberLastName).Returns("Doe");

        // Act
        await _memberPaymentService.AddPaymentAsync(selectedID);

        // Assert
        _paymentsServiceMock.Verify(service => service.AddPaymentToPaymentEntities(
            It.Is<PaymentEntity>(p =>
                p.ID == selectedID &&
                p.FirstName == "John" &&
                p.LastName == "Doe" &&
                p.Amount == "30" &&
                p.Description == "R"
            )), Times.Once);
    }

    [Fact]
    public async Task AddPaymentAsync_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        int selectedID = 1;
        var exception = new Exception("Test exception");
        _paymentsServiceMock.Setup(service => service.AddPaymentToPaymentEntities(It.IsAny<PaymentEntity>()))
            .Throws(exception);

        // Act
        await _memberPaymentService.AddPaymentAsync(selectedID);

        // Assert
        _loggerMock.Verify(logger => logger.LogExceptionAsync(exception, $"{nameof(MemberPaymentService)}.{nameof(MemberPaymentService.AddPaymentAsync)}"), Times.Once);
    }
}