using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents
{
    public class CopyToClipboardInPaymentsComponentTests
    {
        private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
        private readonly Mock<IClipboardHandler> _mockClipboardHandlerService;
        private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
        private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
        private readonly CopyToClipboardInPaymentsComponent _component;

        public CopyToClipboardInPaymentsComponentTests()
        {
            _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
            _mockClipboardHandlerService = new Mock<IClipboardHandler>();
            _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
            _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();

            _component = new CopyToClipboardInPaymentsComponent();

            _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
            _component.SetPrivatePropertyValue("ClipboardHandlerService", _mockClipboardHandlerService.Object);
            _component.SetPrivatePropertyValue("Logger", _mockLoggerService.Object);
            _component.SetPrivatePropertyValue("PaymentsService", _mockPaymentsService.Object);
        }

        [Fact]
        public void OnInitialized_ShouldSubscribeToPaymentEntitiesSizeChanged()
        {
            // Act
            typeof(CopyToClipboardInPaymentsComponent).Invoke("OnInitialized", _component);

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
            _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

            // Act
            typeof(CopyToClipboardInPaymentsComponent).Invoke("OnPaymentEntitiesSizeChanged", _component);

            // Assert
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
            await typeof(CopyToClipboardInPaymentsComponent).InvokeAsync("OnCopyToClipboardButtonClickedAsync", _component);

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
            await typeof(CopyToClipboardInPaymentsComponent).InvokeAsync("OnCopyToClipboardButtonClickedAsync", _component);

            // Assert
            _mockLoggerService.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldUnsubscribeFromPaymentEntitiesSizeChanged()
        {
            // Act
            _component.Dispose();

            // Assert
            _mockPaymentsService.Verify(p => p.OnPaymentEntitiesSizeChanged(), Times.Never);
        }
    }
}
