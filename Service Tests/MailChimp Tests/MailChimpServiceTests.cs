using DBExplorerBlazor.Services;
using MailChimp.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace MailChimp;

public class MailChimpServiceTests
{
    private readonly Mock<IConfiguration> mockConfiguration;
    private readonly MailChimpService mailChimpService;

    public MailChimpServiceTests()
    {
        mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["ConnectionStrings:MailChimpAPIKey"]).Returns("fake-api-key");
        mockConfiguration.Setup(config => config["ConnectionStrings:MailChimpSTPCE-UpdateID"]).Returns("fake-list-id");

        mailChimpService = new MailChimpService(mockConfiguration.Object);
    }

    [Fact]
    public void MailChimpManager_IsInitializedCorrectly()
    {
        // Assert
        Assert.NotNull(mailChimpService.MailChimpManager);
        Assert.IsAssignableFrom<IMailChimpManager>(mailChimpService.MailChimpManager);
    }

    [Fact]
    public void ListID_IsInitializedCorrectly()
    {
        // Assert
        Assert.Equal("fake-list-id", mailChimpService.ListID);
    }


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
}