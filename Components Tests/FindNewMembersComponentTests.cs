using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class FindNewMembersComponentTests
{
    private readonly Mock<IDataManager> _dataManagerMock;
    private readonly Mock<ICrossCuttingFileNameValidationService> _fileNameValidationService;
    private readonly Mock<ICrossCuttingLoadingStateService> _loadingStateServiceMock;
    private readonly Mock<ICrossCuttingLoadingPanelService> _loadingPanelServiceMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<IFindNewMembersExportService> _memberExportServiceMock;
    private readonly FindNewMembersComponent _component;

    public FindNewMembersComponentTests()
    {
        _dataManagerMock = new Mock<IDataManager>();
        _fileNameValidationService = new Mock<ICrossCuttingFileNameValidationService>();
        _loadingStateServiceMock = new Mock<ICrossCuttingLoadingStateService>();
        _loadingPanelServiceMock = new Mock<ICrossCuttingLoadingPanelService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _memberExportServiceMock = new Mock<IFindNewMembersExportService>();
        _component = new FindNewMembersComponent
        {
            DataManager = _dataManagerMock.Object,
            FileNameValidationService = _fileNameValidationService.Object,
            LoadingStateService = _loadingStateServiceMock.Object,
            LoadingPanelService = _loadingPanelServiceMock.Object,
            Logger = _loggerMock.Object,
            FindNewMembersExportService = _memberExportServiceMock.Object,
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Now = DateTime.Now,
            ExcelGrid = new SfGrid<NewMemberEntity>(),
            NewMemberEntitiesBDP = new List<NewMemberEntity>
            {
                new() { ID = 1, FirstName = "John", LastName = "Doe" },
                new() { ID = 2, FirstName = "Jane", LastName = "Smith" }
            }
        };
    }

    [Fact]
    public async Task OnParametersSet2Async_ShouldLoadNewMembers()
    {
        // Arrange
        //var startDate = new DateTime(2023, 1, 1);
        //var endDate = new DateTime(2023, 12, 31);
        var newMembers = new List<NewMemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" },
            new() { ID = 2, FirstName = "Jane", LastName = "Smith" }
        };
        _dataManagerMock
            .Setup(dm => dm.GetNewMembersSPAsync(_component.StartDate, _component.EndDate))
            .ReturnsAsync(newMembers);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _dataManagerMock.Verify(dm => dm.GetNewMembersSPAsync(_component.StartDate, _component.EndDate), Times.Once);
        Assert.Equal(newMembers.Count, _component.NewMemberEntitiesBDP.Count);
        Assert.Equal(newMembers[0].ID, _component.NewMemberEntitiesBDP[0].ID);
        Assert.Equal(newMembers[1].ID, _component.NewMemberEntitiesBDP[1].ID);
    }

    [Fact]
    public async Task OnParametersSet2Async_ShouldHandleException()
    {
        // Arrange
        //var startDate = new DateTime(2023, 1, 1);
        //var endDate = new DateTime(2023, 12, 31);
        var exception = new Exception("Test exception");
        _dataManagerMock
            .Setup(dm => dm.GetNewMembersSPAsync(_component.StartDate, _component.EndDate))
            .ThrowsAsync(exception);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _loggerMock.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldExportMembers()
    {
        // Arrange
        var fileName = "test.xlsx";
        _fileNameValidationService
            .Setup(f => f.ValidateAndAlertAsync(fileName))
            .ReturnsAsync(true);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _memberExportServiceMock.Verify(m => m.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, _component.NewMemberEntitiesBDP), Times.Once);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ShouldHandleException()
    {
        // Arrange
        var fileName = "test.xlsx";
        var exception = new Exception("Test exception");
        _fileNameValidationService
           .Setup(f => f.ValidateAndAlertAsync(fileName))
           .ReturnsAsync(true);
        _memberExportServiceMock
            .Setup(m => m.ExportMembersAsync(fileName, _component.StartDate, _component.EndDate, _component.Now, _component.ExcelGrid, _component.NewMemberEntitiesBDP))
            .ThrowsAsync(exception);

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _loggerMock.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}