using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class RosterRecipientsComponentTests
{
    private readonly RosterRecipientsComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryRosterRecipients> _mockRosterRecipientsRepository;

    public RosterRecipientsComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockRosterRecipientsRepository = new Mock<IRepositoryRosterRecipients>();

        _component = new RosterRecipientsComponent
        {
            Show = _mockShow.Object,
            ExportExcelFileService = _mockExportExcelFileService.Object,
            IsValidFileNameService = _mockIsValidFileNameService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            RosterRecipientsRepository = _mockRosterRecipientsRepository.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_CallsLoadRosterRecipientsAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadRosterRecipientsAndManageUIAsyncCalled = false;
    //    _component.LoadRosterRecipientsAndManageUIAsync = () =>
    //    {
    //        loadRosterRecipientsAndManageUIAsyncCalled = true;
    //        return Task.CompletedTask;
    //    };

    //    // Act
    //    await _component.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadRosterRecipientsAndManageUIAsyncCalled);
    //}

    [Fact]
    public async Task LoadRosterRecipientsAndManageUIAsync_LoadsRecipientsAndSetsTitle()
    {
        // Arrange
        var recipients = new List<RosterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "Jane", LastName = "Smith" }
        };
        _mockRosterRecipientsRepository
            .Setup(repo => repo.GetRosterRecipientsAsync(It.IsAny<int>()))
            .ReturnsAsync(recipients);
        _component.Initialize(30);

        // Act
        await _component.LoadRosterRecipientsAndManageUIAsync();

        // Assert
        Assert.Equal(recipients, _component.RosterRecipientEntitiesBDP);
        Assert.Equal("2 members receiving printed roster with grace period of 30 days", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);
        _component.RosterRecipientEntitiesBDP = new List<RosterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe", FirstNameLastName = "John Doe" },
            new() { FirstName = "Jane", LastName = "Smith", FirstNameLastName = "Jane Smith" }
        };

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync(
            fileName, It.IsAny<List<RosterRecipientEntity>>(), It.IsAny<int>(), It.IsAny<SfGrid<RosterRecipientEntity>>()), Times.Once);
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
        var recipients = new List<RosterRecipientEntity>
        {
            new() { FirstName = "John", LastName = "Doe", FirstNameLastName = "John Doe", Address1 = "123 Main St", City = "Anytown", State = "CA", Zip = "12345" },
            new() { FirstName = "Jane", LastName = "Smith", FirstNameLastName = "Jane Smith", Address1 = "456 Elm St", City = "Othertown", State = "TX", Zip = "67890" }
        };

        // Act
        var result = _component.PrepareSpreadsheetDataForExport(recipients, 30, now);

        // Assert
        Assert.Contains(result, r => r.FirstNameLastName == "John Doe");
        Assert.Contains(result, r => r.FirstNameLastName == "Jane Smith");
        Assert.Contains(result, r => r.Address1 == "123 Main St");
        Assert.Contains(result, r => r.Address1 == "456 Elm St");
        Assert.Contains(result, r => r.City == "Anytown");
        Assert.Contains(result, r => r.City == "Othertown");
        Assert.Contains(result, r => r.State == "CA");
        Assert.Contains(result, r => r.State == "TX");
        Assert.Contains(result, r => r.Zip == "12345");
        Assert.Contains(result, r => r.Zip == "67890");
    }
}