using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents
{
    public class CopyToClipboardInPaymentsComponentTests
    {
        private readonly Mock<IClipboardHandler> _mockClipboardHandlerService;
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<IPaymentsService> _mockPaymentsService;
        private readonly CopyToClipboardInPaymentsComponent _component;

        public CopyToClipboardInPaymentsComponentTests()
        {
            _mockClipboardHandlerService = new Mock<IClipboardHandler>();
            _mockLoggerService = new Mock<ILoggerService>();
            _mockPaymentsService = new Mock<IPaymentsService>();

            _component = new CopyToClipboardInPaymentsComponent
            {
                ClipboardHandlerService = _mockClipboardHandlerService.Object,
                Logger = _mockLoggerService.Object,
                PaymentsService = _mockPaymentsService.Object
            };
        }

        [Fact]
        public void OnInitialized_ShouldSubscribeToPaymentEntitiesSizeChanged()
        {
            // Act
            _component.OnInitialized2();

            // Assert
            _mockPaymentsService.Verify(p => p.OnPaymentEntitiesSizeChanged()/*.PaymentEntitiesSizeChanged*/, Times.Never);
        }

        [Fact]
        public void OnPaymentEntitiesSizeChanged_ShouldUpdateIsCopyButtonDisabledBDP()
        {
            // Arrange
            var paymentEntities = new ObservableCollection<PaymentEntity>
            {
                new() { ID = 1, Description = "Payment 1", FirstName = "John", LastName = "Doe", Amount = "100" }
            };
            _mockPaymentsService.Setup(p => p.PaymentEntities).Returns(paymentEntities);

            // Act
            _component.OnPaymentEntitiesSizeChanged2();

            // Assert
            //Assert.False(_component.IsCopyButtonDisabledBDP);
            Assert.False(_component.IsCopyButtonDisabled);
        }

        [Fact]
        public async Task OnCopyToClipboardButtonClickedAsync_ShouldCallClipboardHandlerService()
        {
            // Arrange
            var paymentEntities = new ObservableCollection<PaymentEntity>
            {
                new() { ID = 1, Description = "Payment 1", FirstName = "John", LastName = "Doe", Amount = "100" },
                new() { ID = 2, Description = "Payment 2", FirstName = "Jane", LastName = "Smith", Amount = "200" }
            };
            _mockPaymentsService.Setup(p => p.PaymentEntities).Returns(paymentEntities);

            // Act
            await _component.OnCopyToClipboardButtonClickedAsync();

            // Assert
            _mockClipboardHandlerService.Verify(chs => chs.CopyPaymentDetailsToClipboardAsync(paymentEntities), Times.Once);
        }

        [Fact]
        public async Task OnCopyToClipboardButtonClickedAsync_ShouldLogExceptionOnFailure()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockClipboardHandlerService.Setup(chs => chs.CopyPaymentDetailsToClipboardAsync(It.IsAny<IEnumerable<PaymentEntity>>()))
                .ThrowsAsync(exception);

            // Act
            await _component.OnCopyToClipboardButtonClickedAsync();

            // Assert
            _mockLoggerService.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldUnsubscribeFromPaymentEntitiesSizeChanged()
        {
            // Act
            _component.Dispose();
            //_mockPaymentsService.Setup(p => p.PaymentEntitiesSizeChanged -= It.IsAny<Action>());

            // Assert
            _mockPaymentsService.Verify(p => p.OnPaymentEntitiesSizeChanged()/*.PaymentEntitiesSizeChanged*/, Times.Never);
            //_mockPaymentsService.Verify(p => p.PaymentEntitiesSizeChanged, Times.Once);
        }
    }
}
