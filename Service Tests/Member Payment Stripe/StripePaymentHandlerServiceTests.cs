using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberPaymentStripe;

public class StripePaymentHandlerServiceTests
{
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly Mock<ICrossCuttingStripeService> _mockStripeService;
    private readonly StripePaymentHandlerService _service;

    public StripePaymentHandlerServiceTests()
    {
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _mockStripeService = new Mock<ICrossCuttingStripeService>();

        _service = new StripePaymentHandlerService(_mockPaymentsService.Object, _mockStripeService.Object);
    }

    [Fact]
    public void AddStripePayment_ShouldAddPayment_WhenStripeEntityExists()
    {
        // Arrange
        var stripeMemberID = 1;
        var stripeEntity = new StripeEntity
        {
            ID = stripeMemberID,
            FirstName = "John",
            LastName = "Doe",
            Payment = "100.00"
        };

        _mockStripeService.Setup(s => s.StripeEntities).Returns(new List<StripeEntity> { stripeEntity });

        // Act
        _service.AddStripePayment(stripeMemberID);

        // Assert
        _mockPaymentsService.Verify(p => p.AddPaymentToPaymentEntities(It.Is<PaymentEntity>(pe =>
            pe.ID == stripeMemberID &&
            pe.FirstName == stripeEntity.FirstName &&
            pe.LastName == stripeEntity.LastName &&
            pe.Amount == stripeEntity.Payment &&
            pe.Description == "R")), Times.Once);
    }

    [Fact]
    public void AddStripePayment_ShouldNotAddPayment_WhenStripeEntityDoesNotExist()
    {
        // Arrange
        var stripeMemberID = 1;

        _mockStripeService.Setup(s => s.StripeEntities).Returns(new List<StripeEntity>());

        // Act
        _service.AddStripePayment(stripeMemberID);

        // Assert
        _mockPaymentsService.Verify(p => p.AddPaymentToPaymentEntities(It.IsAny<PaymentEntity>()), Times.Never);
    }
}