//using DataAccess.Models;
//using DBExplorerBlazor.Interfaces;
//using DBExplorerBlazor3.Services.ExpiredMemberships;
//using Moq;
//using Syncfusion.Blazor.Grids;

//namespace MenuItemComponents;

//public class ExpiredMembershipsDataExportTests
//{
//    private readonly Mock<ICrossCuttingAlertService> mockAlertService = new();
//    private readonly Mock<IExpiredMembershipsExportService> mockExpiredMembershipsExport = new();
//    private readonly Mock<ICrossCuttingIsValidFileNameService> mockIsValidFileNameService = new();
//    private readonly Mock<ICrossCuttingLoggerService> mockLogger = new();
//    private readonly ExpiredMembershipsDataExportService service;

//    public ExpiredMembershipsDataExportTests()
//    {
//        service = new ExpiredMembershipsDataExportService(mockAlertService.Object, mockExpiredMembershipsExport.Object,
//            mockIsValidFileNameService.Object, mockLogger.Object);
//    }

//    [Fact]
//    public async Task ExportDataAsync_ShowsAlert_IfFileNameIsInvalid()
//    {
//        // Arrange
//        string invalidFileName = "invalid/file/name";
//        mockIsValidFileNameService.Setup(s => s.FileNameValid(invalidFileName)).Returns(false);

//        // Act
//        await service.ExportDataAsync(invalidFileName, new List<ExpiredMembershipsEntity>(), DateTime.Now, DateTime.Now, new SfGrid<ExpiredMembershipsEntity>());

//        // Assert
//        mockAlertService.Verify(a => a.InappropriateFileNameAlertUsingFallingMessageBoxAsync(invalidFileName), Times.Once);
//    }

//    [Fact]
//    public async Task ExportDataAsync_CallsExport_IfFileNameIsValid()
//    {
//        // Arrange
//        string validFileName = "validFileName";
//        mockIsValidFileNameService.Setup(s => s.FileNameValid(validFileName)).Returns(true);

//        // Act
//        await service.ExportDataAsync(validFileName, new List<ExpiredMembershipsEntity>(), DateTime.Now, DateTime.Now, new SfGrid<ExpiredMembershipsEntity>());

//        // Assert
//        mockExpiredMembershipsExport.Verify(e => e.ExportDataAsync(validFileName, It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<ExpiredMembershipsEntity>>()), Times.Once);
//    }

//    [Fact]
//    public async Task ExportDataAsync_LogsException_IfExportFails()
//    {
//        // Arrange
//        string validFileName = "validFileName";
//        var exception = new Exception("Test exception");
//        mockIsValidFileNameService.Setup(s => s.FileNameValid(validFileName)).Returns(true);
//        mockExpiredMembershipsExport.Setup(e => e.ExportDataAsync(validFileName, It.IsAny<List<ExpiredMembershipsEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<ExpiredMembershipsEntity>>())).ThrowsAsync(exception);

//        // Act
//        await service.ExportDataAsync(validFileName, new List<ExpiredMembershipsEntity>(), DateTime.Now, DateTime.Now, new SfGrid<ExpiredMembershipsEntity>());

//        // Assert
//        mockLogger.Verify(l => l.LogExceptionAsync(exception, "DataExportService.ExportDataAsync"), Times.Once);
//    }
//}