using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class PaymentToMembershipIDServiceTests
{
    private readonly Mock<IGetPaymentToMembershipID> _dataManagerMock;
    private readonly PaymentToMembershipIDService _service;

    public PaymentToMembershipIDServiceTests()
    {
        _dataManagerMock = new Mock<IGetPaymentToMembershipID>();
        _service = new PaymentToMembershipIDService(_dataManagerMock.Object);
    }

    [Fact]
    public async Task GetPaymentToMembershipIDDictionaryAsync_ShouldReturnDictionary()
    {
        // Arrange
        var paymentToMembershipIDList = new List<PaymentMembershipIDEntity>
        {
            new() { Payment = "Payment1", MembershipID = 1 },
            new() { Payment = "Payment2", MembershipID = 2 }
        };

        _dataManagerMock.Setup(dm => dm.GetPaymentToMembershipIDSPAsync())
            .ReturnsAsync(paymentToMembershipIDList);

        // Act
        var result = await _service.GetPaymentToMembershipIDDictionaryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result["Payment1"]);
        Assert.Equal(2, result["Payment2"]);
    }

    [Fact]
    public async Task GetPaymentToMembershipIDDropDownListAsync_ShouldReturnDropDownList()
    {
        // Arrange
        var paymentToMembershipIDList = new List<PaymentMembershipIDEntity>
        {
            new() { Payment = "Payment1", MembershipID = 1 },
            new() { Payment = "Payment2", MembershipID = 2 }
        };

        _dataManagerMock.Setup(dm => dm.GetPaymentToMembershipIDSPAsync())
            .ReturnsAsync(paymentToMembershipIDList);

        // Act
        var result = await _service.GetPaymentToMembershipIDDropDownListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Item1", result[0].ID);
        Assert.Equal("Payment1", result[0].Text);
        Assert.Equal("Item2", result[1].ID);
        Assert.Equal("Payment2", result[1].Text);
    }
}