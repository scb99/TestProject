using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberPayment;

public class PaymentEntityFactoryServiceTests
{
    private readonly Mock<IMemberNameFinderService> _memberNameFinderServiceMock;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _allMembersInDBServiceMock;
    private readonly PaymentEntityFactoryService _service;

    public PaymentEntityFactoryServiceTests()
    {
        _memberNameFinderServiceMock = new Mock<IMemberNameFinderService>();
        _allMembersInDBServiceMock = new Mock<ICrossCuttingAllMembersInDBService>();
        _service = new PaymentEntityFactoryService(_memberNameFinderServiceMock.Object);
    }

    [Fact]
    public void TryCreatePaymentEntityFromLine_ValidLine_ReturnsTrueAndPaymentEntity()
    {
        // Arrange
        var line = "1 R John Doe 100";
        var expectedPaymentEntity = new PaymentEntity
        {
            ID = 1,
            LastName = "Doe",
            FirstName = "John",
            Description = "R",
            Amount = "100"
        };

        _memberNameFinderServiceMock.Setup(m => m.FindName(1, _allMembersInDBServiceMock.Object))
            .Returns(("Doe", "John"));

        // Act
        var (isValid, paymentEntity) = _service.TryCreatePaymentEntityFromLine(line, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.True(isValid);
        Assert.NotNull(paymentEntity);
        Assert.Equal(expectedPaymentEntity.ID, paymentEntity.ID);
        Assert.Equal(expectedPaymentEntity.LastName, paymentEntity.LastName);
        Assert.Equal(expectedPaymentEntity.FirstName, paymentEntity.FirstName);
        Assert.Equal(expectedPaymentEntity.Description, paymentEntity.Description);
        Assert.Equal(expectedPaymentEntity.Amount, paymentEntity.Amount);
    }

    [Fact]
    public void TryCreatePaymentEntityFromLine_InvalidLineFormat_ReturnsFalseAndNull()
    {
        // Arrange
        var line = "invalid line";

        // Act
        var (isValid, paymentEntity) = _service.TryCreatePaymentEntityFromLine(line, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.False(isValid);
        Assert.Null(paymentEntity);
    }

    [Fact]
    public void TryCreatePaymentEntityFromLine_InvalidDescription_ReturnsFalseAndNull()
    {
        // Arrange
        var line = "1 X 100";

        // Act
        var (isValid, paymentEntity) = _service.TryCreatePaymentEntityFromLine(line, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.False(isValid);
        Assert.Null(paymentEntity);
    }

    [Fact]
    public void TryCreatePaymentEntityFromLine_InvalidMemberId_ReturnsFalseAndNull()
    {
        // Arrange
        var line = "1 R 100";

        _memberNameFinderServiceMock.Setup(m => m.FindName(1, _allMembersInDBServiceMock.Object))
            .Returns((string.Empty, string.Empty));

        // Act
        var (isValid, paymentEntity) = _service.TryCreatePaymentEntityFromLine(line, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.False(isValid);
        Assert.Null(paymentEntity);
    }
}