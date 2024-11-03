using DBExplorerBlazor.Services;
using MailChimp.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Service;

public class MailChimpServiceTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["ConnectionStrings:MailChimpAPIKey"]).Returns("test_api_key");
        mockConfiguration.Setup(c => c["ConnectionStrings:MailChimpSTPCE-UpdateID"]).Returns("test_list_id");

        var mockMailChimpManager = new Mock<IMailChimpManager>();

        // Act
        var service = new MailChimpService(mockConfiguration.Object);

        // Assert
        Assert.NotNull(service.MailChimpManager);
        Assert.Equal("test_list_id", service.ListID);
    }

    // Additional tests would go here, focusing on methods within MailChimpService
    // that interact with MailChimpManager and use ListID.
}