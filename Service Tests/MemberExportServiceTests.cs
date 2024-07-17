using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace Service;

public class MemberExportServiceTests
{
    private readonly Mock<IExportExcelFileService> _mockExportExcelFileService = new();
    private readonly Mock<IFileNameValidationService> _mockFileNameValidationService = new();
    private readonly Mock<IIsValidFileNameService> _mockIsValidFileNameService = new();
    private readonly Mock<ILoggerService> _mockLogger = new();
    private readonly Mock<IMemberDataPreparationService> _mockMemberDataPreparationService = new();
    private readonly MemberExportService _service;

    public MemberExportServiceTests()
    {
        _service = new MemberExportService(_mockExportExcelFileService.Object, _mockFileNameValidationService.Object,
                                           _mockIsValidFileNameService.Object, _mockLogger.Object, _mockMemberDataPreparationService.Object);
    }

    [Fact]
    public async Task ExportMembersAsync_ValidationFails_ExitsEarly()
    {
        // Arrange
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        await _service.ExportMembersAsync("invalid.txt", DateTime.Now, DateTime.Now, DateTime.Now, null, new List<NewMemberEntity>());

        // Assert
        _mockExportExcelFileService.Verify(s => s.DownloadTextDocumentToUsersMachineAsync<NewMemberEntity>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        _mockExportExcelFileService.Verify(s => s.DownloadSpreadsheetDocumentToUsersMachineAsync<NewMemberEntity>(It.IsAny<string>(), It.IsAny<List<NewMemberEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<NewMemberEntity>>()), Times.Never);
    }

    [Fact]
    public async Task ExportMembersAsync_ValidatesAndExportsTextFile_Success()
    {
        // Arrange
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockIsValidFileNameService.Setup(s => s.IsFileNameATextFile(It.IsAny<string>())).Returns(true);
        _mockMemberDataPreparationService.Setup(s => s.PrepareDataForExport(It.IsAny<List<NewMemberEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns(new List<NewMemberEntity>());

        // Act
        await _service.ExportMembersAsync("valid.txt", DateTime.Now, DateTime.Now, DateTime.Now, null, new List<NewMemberEntity>());

        // Assert
        _mockExportExcelFileService.Verify(s => s.DownloadTextDocumentToUsersMachineAsync<NewMemberEntity>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task ExportMembersAsync_ValidatesAndExportsSpreadsheet_Success()
    {
        // Arrange
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockIsValidFileNameService.Setup(s => s.IsFileNameATextFile(It.IsAny<string>())).Returns(false);
        _mockMemberDataPreparationService.Setup(s => s.PrepareDataForExport(It.IsAny<List<NewMemberEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns(new List<NewMemberEntity>());

        // Act
        await _service.ExportMembersAsync("valid.xlsx", DateTime.Now, DateTime.Now, DateTime.Now, null, new List<NewMemberEntity>());

        // Assert
        _mockExportExcelFileService.Verify(s => s.DownloadSpreadsheetDocumentToUsersMachineAsync<NewMemberEntity>(It.IsAny<string>(), It.IsAny<List<NewMemberEntity>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SfGrid<NewMemberEntity>>()), Times.Once);
    }

    [Fact]
    public async Task ExportMembersAsync_ThrowsException_LogsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockFileNameValidationService.Setup(s => s.ValidateAndAlertAsync(It.IsAny<string>())).Throws(exception);

        // Act
        await _service.ExportMembersAsync("test.txt", DateTime.Now, DateTime.Now, DateTime.Now, null, new List<NewMemberEntity>());

        // Assert
        _mockLogger.Verify(s => s.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

}