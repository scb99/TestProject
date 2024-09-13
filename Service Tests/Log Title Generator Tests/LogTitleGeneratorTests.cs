namespace LogTitleGenerator;

public class LogTitleGeneratorTests
{
    [Theory]
    [InlineData(1, "2023-01-01", "2023-01-31", "1 log entry between 1/1/2023 and 1/31/2023")]
    [InlineData(2, "2023-02-01", "2023-02-28", "2 log entries between 2/1/2023 and 2/28/2023")]
    public void GenerateLogTitle_ReturnsCorrectTitle(int logCount, string startDate, string endDate, string expectedTitle)
    {
        // Arrange
        var generator = new DBExplorerBlazor.Services.LogTitleGeneratorService();
        var start = DateTime.Parse(startDate);
        var end = DateTime.Parse(endDate);

        // Act
        var result = generator.GenerateLogTitle(logCount, start, end);

        // Assert
        Assert.Equal(expectedTitle, result);
    }
}