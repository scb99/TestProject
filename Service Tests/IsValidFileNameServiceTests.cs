using DBExplorerBlazor.Services;

namespace Service;

public class IsValidFileNameServiceTests
{
    private readonly IsValidFileNameService _isValidFileNameService;

    public IsValidFileNameServiceTests()
    {
        _isValidFileNameService = new IsValidFileNameService();
    }

    [Theory]
    [InlineData("validfile.csv", true)]
    [InlineData("validfile.txt", true)]
    [InlineData("validfile.xlsx", true)]
    [InlineData("invalidfile.doc", false)]
    [InlineData("invalidfile", false)]
    [InlineData("invalidfile.", false)]
    [InlineData(".hiddenfile.csv", false)]
    [InlineData("valid-file.csv", true)]
    [InlineData("valid_file.csv", true)]
    [InlineData("valid file.csv", true)]
    [InlineData("validfile.csv.exe", false)]
    public void FileNameValid_ShouldReturnExpectedResult(string fileName, bool expectedResult)
    {
        // Act
        var result = _isValidFileNameService.FileNameValid(fileName);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("validfile.txt", true)]
    [InlineData("validfile.csv", false)]
    [InlineData("validfile.xlsx", false)]
    [InlineData("invalidfile.doc", false)]
    public void IsFileNameATextFile_ShouldReturnExpectedResult(string fileName, bool expectedResult)
    {
        // Act
        var result = _isValidFileNameService.IsFileNameATextFile(fileName);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
