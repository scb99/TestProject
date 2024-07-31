using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace PaymentDetailsFormatterNS;

public class PaymentDetailsFormatterTests
{
    [Fact]
    public void FormatForClipboard_GivenEmptyList_ReturnsEmptyString()
    {
        // Arrange
        var formatter = new PaymentDetailsFormatter();
        var paymentEntities = new List<PaymentEntity>();

        // Act
        var result = formatter.FormatForClipboard(paymentEntities);

        // Assert
        Assert.Equal("\r\n renewal", result);
    }

    [Fact]
    public void FormatForClipboard_GivenListOfPaymentEntities_FormatsCorrectly()
    {
        // Arrange
        var formatter = new PaymentDetailsFormatter();
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe", Amount = "100", Description = "Test Payment 1" },
            new() { ID = 2, FirstName = "Jane", LastName = "Smith", Amount = "200", Description = "Test Payment 2" }
        };
        var expectedFormat = "1 Test Payment 1 Doe, John 100\r\n2 Test Payment 2 Smith, Jane 200\r\nJohn Doe, Jane Smith renewal";

        // Act
        var result = formatter.FormatForClipboard(paymentEntities);

        // Assert
        Assert.Equal(expectedFormat, result);
    }
}