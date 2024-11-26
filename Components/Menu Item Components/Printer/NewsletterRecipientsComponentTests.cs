using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class NewsletterRecipientsComponentTests
{
    private readonly NewsletterRecipientsComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryNewsletterRecipients> _mockNewsletterRecipientsRepository;

    public NewsletterRecipientsComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockNewsletterRecipientsRepository = new Mock<IRepositoryNewsletterRecipients>();

        _component = new NewsletterRecipientsComponent
        {
            Show = _mockShow.Object,
            ExportExcelFileService = _mockExportExcelFileService.Object,
            IsValidFileNameService = _mockIsValidFileNameService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            NewsletterRecipientsRepository = _mockNewsletterRecipientsRepository.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_CallsLoadNewsletterRecipientsAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadNewsletterRecipientsAndManageUIAsyncCalled = false;
    //    _component.LoadNewsletterRecipientsAndManageUIAsync = () =>
    //    {
    //        loadNewsletterRecipientsAndManageUIAsyncCalled = true;
    //        return Task.CompletedTask;
    //    };

    //    // Act
    //    await _component.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadNewsletterRecipientsAndManageUIAsyncCalled);
    //}

    [Fact]
    public async Task LoadNewsletterRecipientsAndManageUIAsync_LoadsRecipientsAndSetsTitle()
    {
        // Arrange
        var recipients = new List<NewsletterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "Jane", LastName = "Smith" }
        };
        _mockNewsletterRecipientsRepository
            .Setup(repo => repo.GetNewsletterRecipientsAsync(It.IsAny<int>()))
            .ReturnsAsync(recipients);
        _component.SetPublicPropertyValue("GracePeriod", 30);

        // Act
        await _component.LoadNewsletterRecipientsAndManageUIAsync();

        // Assert
        Assert.Equal(recipients, _component.NewsletterRecipientEntitiesBDP);
        Assert.Equal("2 members receiving printed newsletter with grace period of 30 days", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);
        _component.NewsletterRecipientEntitiesBDP = new List<NewsletterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe", Name = "John Doe", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", Month="December", Day = "25", Year = "2024", RenewalDate = "December 25, 2025" },
            new() { FirstName = "Jane", LastName = "Smith", Name = "Jane Smith", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", Month="December", Day = "25", Year = "2025", RenewalDate = "October 1, 2024" }
        };

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync(
            fileName, It.IsAny<List<NewsletterRecipientEntity>>(), It.IsAny<int>(), It.IsAny<SfGrid<NewsletterRecipientEntity>>()), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_InvalidFileName_ShowsAlert()
    {
        // Arrange
        var fileName = "invalidFileName";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(false);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockShow.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
    }

    [Fact]
    public void PrepareSpreadsheetDataForExport_GeneratesCorrectData()
    {
        // Arrange
        var now = DateTime.Now;
        var recipients = new List<NewsletterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe", Name = "John Doe", Address1 = "123 Main St", Address2 = "", City = "Anytown", State = "CA", Zip = "12345", Month = "December", Day = "31", Year = "2023" },
            new() { FirstName = "Jane", LastName = "Smith", Name = "Jane Smith", Address1 = "456 Elm St", Address2 = "", City = "Othertown", State = "TX", Zip = "67890", Month = "December", Day = "31", Year = "2023" }
        };

        // Act
        var result = _component.PrepareSpreadsheetDataForExport(recipients, 30, now);

        // Assert
        Assert.Contains(result, r => r.Name == "John Doe");
        Assert.Contains(result, r => r.Name == "Jane Smith");
        Assert.Contains(result, r => r.Street == "123 Main St");
        Assert.Contains(result, r => r.Street == "456 Elm St");
        Assert.Contains(result, r => r.City == "Anytown");
        Assert.Contains(result, r => r.City == "Othertown");
        Assert.Contains(result, r => r.StateAndZip == "CA 12345");
        Assert.Contains(result, r => r.StateAndZip == "TX 67890");
        Assert.Contains(result, r => r.RenewalDate == "Renewal date: December 31, 2023");
    }
}