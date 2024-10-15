using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class MembersWithoutEmailComponentTests
{
    private readonly MembersWithoutEmailComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryMembersWithoutEmailAddress> _mockMembersWithoutEmailAddressRepository;

    public MembersWithoutEmailComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockMembersWithoutEmailAddressRepository = new Mock<IRepositoryMembersWithoutEmailAddress>();

        _component = new MembersWithoutEmailComponent
        {
            Show = _mockShow.Object,
            ExportExcelFileService = _mockExportExcelFileService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            IsValidFileNameService = _mockIsValidFileNameService.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            MembersWithoutEmailAddressRepository = _mockMembersWithoutEmailAddressRepository.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_CallsLoadMembersWithoutEmailAddressesAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadMembersWithoutEmailAddressesAndManageUIAsyncCalled = false;
    //    _component.LoadMembersWithoutEmailAddressesAndManageUIAsync = () =>
    //    {
    //        loadMembersWithoutEmailAddressesAndManageUIAsyncCalled = true;
    //        return Task.CompletedTask;
    //    };

    //    // Act
    //    await _component.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadMembersWithoutEmailAddressesAndManageUIAsyncCalled);
    //}

    [Fact]
    public async Task LoadMembersWithoutEmailAddressesAndManageUIAsync_LoadsMembersAndSetsTitle()
    {
        // Arrange
        var members = new List<MembersWithoutEmailEntity>
        {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "Jane", LastName = "Smith" }
        };
        _mockMembersWithoutEmailAddressRepository
            .Setup(repo => repo.GetMembersWithoutEmailAddressesAsync(It.IsAny<int>()))
            .ReturnsAsync(members);

        // Act
        await _component.LoadMembersWithoutEmailAddressesAndManageUIAsync();

        // Assert
        Assert.Equal(members, _component.MembersWithoutEmailEntitiesBDP);
        Assert.Equal("2 members without email addresses with grace period of 0 days", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);
        _component.MembersWithoutEmailEntitiesBDP = new List<MembersWithoutEmailEntity>
        {
            new() { FirstName = "John", LastName = "Doe", Name = "John Doe", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", Month = "12", Day = "1", Year = "2024" },
            new() { FirstName = "Jane", LastName = "Smith", Name = "Jane Smith", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", Month = "12", Day = "1", Year = "2024" }
        };

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync(
            fileName, It.IsAny<List<MembersWithoutEmailEntity>>(), It.IsAny<int>(), It.IsAny<SfGrid<MembersWithoutEmailEntity>>()), Times.Once);
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
}