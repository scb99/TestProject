﻿using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class SoonToExpireMembershipsComponentTests
{
    private readonly SoonToExpireMembershipsComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingExportExcelFileService> _mockExportExcelFileService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingIsValidFileNameService> _mockIsValidFileNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<IRepositorySoonToExpireMemberships> _mockSoonToExpireMembershipsRepository;

    public SoonToExpireMembershipsComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExportExcelFileService = new Mock<ICrossCuttingExportExcelFileService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockSoonToExpireMembershipsRepository = new Mock<IRepositorySoonToExpireMemberships>();

        _component = new SoonToExpireMembershipsComponent
        {
            Show = _mockShow.Object,
            ExportExcelFileService = _mockExportExcelFileService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            IsValidFileNameService = _mockIsValidFileNameService.Object,
            SystemTimeService = _mockSystemTimeService.Object,
            SoonToExpireMembershipsRepository = _mockSoonToExpireMembershipsRepository.Object
        };
    }

    //[Fact]
    //public async Task OnParametersSetAsync_CallsLoadSoonToExpireMembershipsAndManageUIAsync()
    //{
    //    // Arrange
    //    var loadSoonToExpireMembershipsAndManageUIAsyncCalled = false;
    //    _component.LoadSoonToExpireMembershipsAndManageUIAsync = () =>
    //    {
    //        loadSoonToExpireMembershipsAndManageUIAsyncCalled = true;
    //        return Task.CompletedTask;
    //    };

    //    // Act
    //    await _component.OnParametersSet2Async();

    //    // Assert
    //    Assert.True(loadSoonToExpireMembershipsAndManageUIAsyncCalled);
    //}

    [Fact]
    public async Task LoadSoonToExpireMembershipsAndManageUIAsync_LoadsMembershipsAndSetsTitle()
    {
        // Arrange
        var memberships = new List<SoonToExpireMembershipsEntity>
        {
            new() { Name = "John Doe", RenewalDate = "2023-12-31" },
            new() { Name = "Jane Smith", RenewalDate = "2023-12-31" }
        };
        _mockSoonToExpireMembershipsRepository
            .Setup(repo => repo.GetSoonToExpireMembershipsAsync(It.IsAny<int>()))
            .ReturnsAsync(memberships);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);
        _component.Initialize(DateTime.Now.AddDays(30));

        // Act
        await _component.LoadSoonToExpireMembershipsAndManageUIAsync();

        // Assert
        Assert.Equal(memberships, _component.SoonToExpireMembershipsEntitiesBDP);
        Assert.Contains("membership", _component.TitleBDP);
    }

    [Fact]
    public async Task OnClickExportSpreadsheetDataAsync_ValidFileName_ExportsData()
    {
        // Arrange
        var fileName = "validFileName.xlsx";
        _mockIsValidFileNameService.Setup(service => service.FileNameValid(fileName)).Returns(true);
        _mockSystemTimeService.Setup(service => service.Now).Returns(DateTime.Now);
        _component.SoonToExpireMembershipsEntitiesBDP = new List<SoonToExpireMembershipsEntity>()
        {
            new() { FirstName = "John", LastName = "Doe", Name = "John Doe", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", RenewalDate = "December 25, 2025" },
            new() { FirstName = "Jane", LastName = "Smith", Name = "Jane Smith", Address1 = "5", Address2 = "", City = "A", State = "MN", Zip = "55343", RenewalDate = "October 1, 2024" }
        };

        // Act
        await _component.OnClickExportSpreadsheetDataAsync(fileName);

        // Assert
        _mockExportExcelFileService.Verify(service => service.DownloadSpreadsheetDocumentToUsersMachineAsync(
            fileName, It.IsAny<List<SoonToExpireMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<SoonToExpireMembershipsEntity>>()), Times.Once);
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