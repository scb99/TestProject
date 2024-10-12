using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class RosterForICTComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockFileNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositoryRosterForICT> _mockICTRosterRepository;
    private readonly RosterForICTComponent _component;

    public RosterForICTComponentTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockExportService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockICTRosterRepository = new Mock<IRepositoryRosterForICT>();

        _component = new RosterForICTComponent
        {
            Show = _mockAlertService.Object,
            ExportExcelFileService = _mockExportService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLoggerService.Object,
            IsValidFileNameService = _mockFileNameService.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            ICTRosterRepository = _mockICTRosterRepository.Object,
            GracePeriod = 30
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsICTRosterEntitiesAndTitle()
    {
        // Arrange
        var members = new List<ICTRosterEntity> { new() };
        _mockICTRosterRepository.Setup(repo => repo.GetRosterForICTAsync(_component.GracePeriod))
                                .ReturnsAsync(members);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(members, _component.ICTRosterEntitiesBDP);
        Assert.Equal($"{members.Count} ICT roster member{(members.Count == 1 ? "" : "s")} with grace period {_component.GracePeriod} days", _component.TitleBDP);
    }

    [Fact]
    public async Task LoadICTRosterMembersAndManageUIAsync_SetsLoadingStateCorrectly()
    {
        // Arrange
        var members = new List<ICTRosterEntity> { new() };
        _mockICTRosterRepository.Setup(repo => repo.GetRosterForICTAsync(_component.GracePeriod))
                                .ReturnsAsync(members);

        // Act
        await _component.LoadICTRosterMembersAndManageUIAsync();

        // Assert
        Assert.False(_component.LoadingBDP);
        Assert.Equal(members, _component.ICTRosterEntitiesBDP);
        Assert.Equal($"{members.Count} ICT roster member{(members.Count == 1 ? "" : "s")} with grace period {_component.GracePeriod} days", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_CallsExportService()
    {
        // Arrange
        var fileName = "test.txt";
        var members = new List<ICTRosterEntity> { new() };
        _component.ICTRosterEntitiesBDP = members;
        _mockFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockFileNameService.Setup(service => service.IsFileNameATextFile(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportService.Verify(service => service.DownloadTextDocumentToUsersMachineAsync<ICTRosterEntity>(fileName, It.IsAny<string>(), _component.GracePeriod), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShowsAlertForInvalidFileName()
    {
        // Arrange
        var fileName = "invalid_file_name";
        _mockFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(false);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockAlertService.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(fileName), Times.Once);
    }

    [Fact]
    public async Task LoadICTRosterMembersAndManageUIAsync_ShowsAndHidesLoadingPanel()
    {
        // Arrange
        var members = new List<ICTRosterEntity> { new() };
        _mockICTRosterRepository.Setup(repo => repo.GetRosterForICTAsync(_component.GracePeriod))
                                .ReturnsAsync(members);

        // Act
        await _component.LoadICTRosterMembersAndManageUIAsync();

        // Assert
        _mockLoadingPanelService.Verify(service => service.ShowLoadingPanelAsync(), Times.Once);
        Assert.False(_component.LoadingBDP);
    }
}