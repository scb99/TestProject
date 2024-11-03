//using DataAccess.Models;
//using DBExplorerBlazor.Interfaces;
//using DBExplorerBlazor.Services;
//using Moq;
//using System.Collections.ObjectModel;

//namespace MemberPayment;

//public class RemoveMemberFromPaymentsServiceTests
//{
//    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
//    private readonly Mock<ICrossCuttingStripeService> _stripeServiceMock;
//    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
//    private readonly RemoveMemberFromPaymentsService _removeMemberFromPaymentsService;

//    public RemoveMemberFromPaymentsServiceTests()
//    {
//        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
//        _stripeServiceMock = new Mock<ICrossCuttingStripeService>();
//        _loggerMock = new Mock<ICrossCuttingLoggerService>();
//        _removeMemberFromPaymentsService = new RemoveMemberFromPaymentsService(
//            _paymentsServiceMock.Object,
//            _stripeServiceMock.Object,
//            _loggerMock.Object);
//    }

//    [Fact]
//    public async Task RemoveMemberFromPaymentsAsync_RemovesMemberFromPayments()
//    {
//        // Arrange
//        _paymentsServiceMock.Setup(p => p.RowIndexOfPayment).Returns(0);
//        ObservableCollection<PaymentEntity> temp = new()
//        {
//            new PaymentEntity()
//        };
//        _paymentsServiceMock.Setup(p => p.PaymentEntities).Returns(temp);

//        // Act
//        await _removeMemberFromPaymentsService.RemoveMemberFromPaymentsAsync();

//        // Assert
//        //_paymentsServiceMock.Verify(p => p.PaymentEntities.RemoveAt(0), Times.Once);
//        _paymentsServiceMock.Verify(p => p.OnPaymentEntitiesSizeChanged(), Times.Once);
//        _paymentsServiceMock.Verify(p => p.OnTitleChanged(), Times.Once);
//        _stripeServiceMock.VerifySet(s => s.StripeMemberID = 0, Times.Once);
//    }

//    //[Fact]
//    //public async Task RemoveMemberFromPaymentsAsync_LogsException_WhenExceptionIsThrown()
//    //{
//    //    // Arrange
//    //    //var exception = new Exception("Test exception");
//    //    _paymentsServiceMock.Setup(p => p.RowIndexOfPayment).Returns(0);
//    //    ObservableCollection<PaymentEntity> temp = new();
//    //    //{
//    //    //    new PaymentEntity()
//    //    //};
//    //    _paymentsServiceMock.Setup(p => p.PaymentEntities).Returns(temp);
//    //    //// _paymentsServiceMock.Setup(p => p.PaymentEntities.RemoveAt(It.IsAny<int>())).Throws(exception);
//    //    //_paymentsServiceMock.Setup(p => p.PaymentEntities[0]).Throws(exception);

//    //    // Act
//    //    //await _removeMemberFromPaymentsService.RemoveMemberFromPaymentsAsync();

//    //    // Assert
//    //    await Assert.ThrowsAsync<Exception>(() => _removeMemberFromPaymentsService.RemoveMemberFromPaymentsAsync());
//    //    //_loggerMock.Verify(l => l.LogExceptionAsync(exception, nameof(RemoveMemberFromPaymentsService)), Times.Once);
//    //}
//}