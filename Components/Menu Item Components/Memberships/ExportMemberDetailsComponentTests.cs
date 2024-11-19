using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class ExportMemberDetailsComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryCompleteRoster> _mockRosterRepository;
    private readonly ExportMemberDetailsComponent _component;

    public ExportMemberDetailsComponentTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockRosterRepository = new Mock<IRepositoryCompleteRoster>();

        _component = new ExportMemberDetailsComponent();

        _component.SetPrivatePropertyValue("Show", _mockAlertService.Object);
        _component.SetPrivatePropertyValue("ExportExcelFileService", _mockExportExcelFileService.Object);
        _component.SetPrivatePropertyValue("LoadingPanelService", _mockLoadingPanelService.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLoggerService.Object);
        _component.SetPrivatePropertyValue("IsValidFileNameService", _mockIsValidFileNameService.Object);
        _component.SetPrivatePropertyValue("SystemTimeService", _mockSystemTimeService.Object);
        _component.SetPrivatePropertyValue("RosterRepositroy", _mockRosterRepository.Object);
    }

    [Fact]
    public async Task OnParametersSetAsync_LoadsMemberDetails_SetsTitle()
    {
        // Arrange
        var gracePeriod = 30;
        var memberDetails = new List<CompleteRosterEntity>
        {
            new() { Name = "John Doe" }
        };

        _component.SetPublicPropertyValue("GracePeriod", gracePeriod);
        _mockRosterRepository.Setup(r => r.GetCompleteRosterAsync(gracePeriod)).ReturnsAsync(memberDetails);

        // Act
        await typeof(ExportMemberDetailsComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(memberDetails, _component.GetPrivatePropertyValue <List<CompleteRosterEntity>>("MemberDetailsEntitiesBDP"));
        Assert.Equal("1 member detail with grace period of 30 days", _component.GetPrivatePropertyValue<string>("TitleBDP"));
        _mockLoggerService.Verify(l => l.LogResultAsync("Displayed Member Details with gracePeriod of 30 days"), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        var gracePeriod = 30;
        var now = DateTime.Now;
        var memberDetails = new List<CompleteRosterEntity>
        {
            new() { Name = "John Doe", FirstName = "John", LastName = "Doe", Address1 ="123 Elm", Address2 = "", City = "Austin", BirthDate = "12/25/1951" }
        };

        _component.SetPrivatePropertyValue("MemberDetailsEntitiesBDP", memberDetails);
        _component.SetPublicPropertyValue("GracePeriod", gracePeriod);
        _mockIsValidFileNameService.Setup(s => s.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(s => s.Now).Returns(now);

        // Act
        await typeof(ExportMemberDetailsComponent).InvokeAsync("OnClickExportSpreadsheetDataAsync", _component, fileName);

        // Assert
        _mockExportExcelFileService.Verify(e => e.DownloadSpreadsheetDocumentToUsersMachineAsync(fileName, It.IsAny<List<CompleteRosterEntity>>(), gracePeriod, It.IsAny<SfGrid<CompleteRosterEntity>>()), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_InvalidFileName_ShowsAlert()
    {
        // Arrange
        var fileName = "invalidFileName";
        _mockIsValidFileNameService.Setup(s => s.FileNameValid(fileName)).Returns(false);

        // Act
        await typeof(ExportMemberDetailsComponent).InvokeAsync("OnClickExportSpreadsheetDataAsync", _component, fileName);

        // Assert
        _mockAlertService.Verify(a => a.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
    }
}