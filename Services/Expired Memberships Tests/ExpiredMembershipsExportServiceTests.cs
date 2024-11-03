using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace ExpiredMemberships;

public class ExpiredMembershipsExportServiceTests
{
    [Fact]
    public async Task ExportDataAsync_CallsPrepareDataForExportAsync_WithCorrectParameters()
    {
        // Arrange
        var mockPrepareDataService = new Mock<IExpiredMembershipsPrepareDataService>();
        var mockExportService = new Mock<ICrossCuttingExportExcelFileService>();
        var service = new ExpiredMembershipsExportService(mockPrepareDataService.Object, mockExportService.Object);

        var fileName = "test.xlsx";
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var expiredMemberships = new List<ExpiredMembershipsEntity>();
        var excelGrid = new SfGrid<ExpiredMembershipsEntity>();

        mockPrepareDataService.Setup(x => x.PrepareDataForExportAsync(It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<ExpiredMembershipsEntity>());

        // Act
        await service.ExportDataAsync(fileName, expiredMemberships, startDate, endDate, excelGrid);

        // Assert
        mockPrepareDataService.Verify(x => x.PrepareDataForExportAsync(expiredMemberships, startDate, endDate), Times.Once);
    }

    [Fact]
    public async Task ExportDataAsync_CallsDownloadSpreadsheetDocumentToUsersMachineAsync_WithCorrectParameters()
    {
        // Arrange
        var mockPrepareDataService = new Mock<IExpiredMembershipsPrepareDataService>();
        var mockExportService = new Mock<ICrossCuttingExportExcelFileService>();
        var service = new ExpiredMembershipsExportService(mockPrepareDataService.Object, mockExportService.Object);

        var fileName = "test.xlsx";
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var expiredMemberships = new List<ExpiredMembershipsEntity>();
        var excelGrid = new SfGrid<ExpiredMembershipsEntity>();

        var preparedData = new List<ExpiredMembershipsEntity> { new() };
        mockPrepareDataService.Setup(x => x.PrepareDataForExportAsync(It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(preparedData);

        // Act
        await service.ExportDataAsync(fileName, expiredMemberships, startDate, endDate, excelGrid);

        // Assert
        mockExportService.Verify(x => x.DownloadSpreadsheetDocumentToUsersMachineAsync<ExpiredMembershipsEntity>(fileName, preparedData, startDate, endDate, excelGrid), Times.Once);
    }
}
