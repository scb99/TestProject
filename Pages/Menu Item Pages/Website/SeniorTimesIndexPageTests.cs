using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using Moq;

namespace MenuItemPages;

public class SeniorTimesIndexPageTests
{
    private readonly Mock<ICrossCuttingAlertService> mockAlertService;
    private readonly Mock<IJSRuntime> mockJSRuntime;
    private readonly Mock<ICrossCuttingLoggerService> mockLoggerService;
    private readonly Mock<IWebHostEnvironment> mockHostingEnv;
    private readonly SeniorTimesIndexPage page;

    public SeniorTimesIndexPageTests()
    {
        mockAlertService = new Mock<ICrossCuttingAlertService>();
        mockJSRuntime = new Mock<IJSRuntime>();
        mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        mockHostingEnv = new Mock<IWebHostEnvironment>();
        page = new SeniorTimesIndexPage
        {
            Show = mockAlertService.Object,
            JS = mockJSRuntime.Object,
            Logger = mockLoggerService.Object,
            HostingEnv = mockHostingEnv.Object
        };
    }

    //[Fact]
    //public async Task OnChangeAsync_UploadsCsvFileSuccessfully()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    //fileMock.Setup(f => f.FileInfo.Name).Returns("test.csv");
    //    fileMock.Setup(f => f.Stream).Returns(new MemoryStream(Encoding.UTF8.GetBytes("test content")));
    //    var args = new UploadChangeEventArgs { Files = new List<UploadFiles> { fileMock.Object } };
    //    mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

    //    // Act
    //    await page.OnChangeAsync(args);

    //    // Assert
    //    mockAlertService.Verify(s => s.AlertUsingFallingMessageBoxAsync("Successfully uploaded test.csv!"), Times.Once);
    //}

    //[Fact]
    //public async Task OnChangeAsync_DoesNotUploadNonCsvFile()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    //fileMock.Setup(f => f.FileInfo.Name).Returns("test.txt");
    //    var args = new UploadChangeEventArgs { Files = new List<UploadFiles> { fileMock.Object } };

    //    // Act
    //    await page.OnChangeAsync(args);

    //    // Assert
    //    mockAlertService.Verify(s => s.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Never);
    //}

    //[Fact]
    //public async Task OnChangeAsync_LogsExceptionOnError()
    //{
    //    // Arrange
    //    var fileMock = new Mock<UploadFiles>();
    //    //fileMock.Setup(f => f.FileInfo.Name).Returns("test.csv");
    //    fileMock.Setup(f => f.Stream).Throws(new IOException("Test exception"));
    //    var args = new UploadChangeEventArgs { Files = new List<UploadFiles> { fileMock.Object } };
    //    mockHostingEnv.Setup(env => env.ContentRootPath).Returns("C:\\TestPath");

    //    // Act
    //    await page.OnChangeAsync(args);

    //    // Assert
    //    mockLoggerService.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    //}

    //[Fact]
    //public async Task OnProcessCSVClickAsync_ProcessesValidCsvFile()
    //{
    //    // Arrange
    //    page.GetType().GetField("path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(page, "test.csv");
    //    var fileContent = new[] { "Category,Description,Year-Month,Page", "TestCategory,TestDescription,2023-01,1" };
    //    var fileLinesField = page.GetType().GetField("fileLines", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //    fileLinesField.SetValue(page, new List<FileLinesModel>());
    //    var htmlFileLinesField = page.GetType().GetField("htmlFileLines", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //    htmlFileLinesField.SetValue(page, new List<FileLinesModel>());

    //    // Act
    //    await page.OnProcessCSVClickAsync();

    //    // Assert
    //    var fileLines = (List<FileLinesModel>)fileLinesField.GetValue(page);
    //    var htmlFileLines = (List<FileLinesModel>)htmlFileLinesField.GetValue(page);
    //    Assert.Single(fileLines);
    //    Assert.NotEmpty(htmlFileLines);
    //    //mockJSRuntime.Verify(js => js.CopyToClipboardAsync(It.IsAny<string>()), Times.Once);
    //    mockAlertService.Verify(s => s.AlertUsingFallingMessageBoxAsync("Generated HTML copied to Clipboard!"), Times.Once);
    //}

    [Fact]
    public async Task OnProcessCSVClickAsync_HandlesInvalidFilePath()
    {
        // Arrange
        page.GetType().GetField("path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(page, string.Empty);

        // Act
        await page.OnProcessCSVClickAsync();

        // Assert
        mockAlertService.Verify(s => s.AlertUsingFallingMessageBoxAsync("Please perform Step 1!"), Times.Once);
    }

    //[Fact]
    //public async Task OnProcessCSVClickAsync_LogsExceptionOnError()
    //{
    //    // Arrange
    //    page.GetType().GetField("path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(page, "test.csv");
    //    var fileLinesField = page.GetType().GetField("fileLines", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //    fileLinesField.SetValue(page, new List<FileLinesModel>());
    //    var htmlFileLinesField = page.GetType().GetField("htmlFileLines", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //    htmlFileLinesField.SetValue(page, new List<FileLinesModel>());
    //    page.GetType().GetMethod("ReadFile", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(page, null);

    //    // Act
    //    await page.OnProcessCSVClickAsync();

    //    // Assert
    //    mockLoggerService.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    //}

    [Fact]
    public void ReadFile_ReadsValidCsvFile()
    {
        // Arrange
        var pathField = page.GetType().GetField("path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        pathField.SetValue(page, "test.csv");
        File.WriteAllText("test.csv", "Category,Description,Year-Month,Page\nTestCategory,TestDescription,2023-01,1");

        // Act
        var result = page.ReadFile();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        File.Delete("test.csv");
    }

    [Fact]
    public void ReadFile_ReturnsNullForNonExistentFile()
    {
        // Arrange
        var pathField = page.GetType().GetField("path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        pathField.SetValue(page, "nonexistent.csv");

        // Act
        var result = page.ReadFile();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseIndexFile_ParsesValidCsvFile()
    {
        // Arrange
        var fileContent = new[] { "Category,Description,Year-Month,Page", "TestCategory,TestDescription,2023-01,1" };

        // Act
        var result = SeniorTimesIndexPage.ParseIndexFile(fileContent);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Single(result["TestCategory"]);
    }

    [Fact]
    public void ProcessLine_ProcessesValidLine()
    {
        // Arrange
        var input = "TestCategory,TestDescription,2023-01,1";

        // Act
        var result = SeniorTimesIndexPage.ProcessLine(input);

        // Assert
        Assert.Equal(4, result.Length);
        Assert.Equal("TestCategory", result[0]);
        Assert.Equal("TestDescription", result[1]);
        Assert.Equal("2023-01", result[2]);
        Assert.Equal("1", result[3]);
    }

    [Fact]
    public void ProcessLine_HandlesQuotesAndCommas()
    {
        // Arrange
        var input = "\"Test, Category\",\"Test, Description\",\"2023-01\",\"1\"";

        // Act
        var result = SeniorTimesIndexPage.ProcessLine(input);

        // Assert
        Assert.Equal(4, result.Length);
        Assert.Equal("Test, Category", result[0]);
        Assert.Equal("Test, Description", result[1]);
        Assert.Equal("2023-01", result[2]);
        Assert.Equal("1", result[3]);
    }
}