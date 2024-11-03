using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;

namespace MemberPayment;

public class TextLineSplitterServiceTests
{
    private readonly ITextLineSplitterService _service;

    public TextLineSplitterServiceTests()
    {
        _service = new TextLineSplitterService();
    }

    [Theory]
    [InlineData("line1\r\nline2\r\nline3", new[] { "line1", "line2", "line3" })]
    [InlineData("line1\rline2\rline3", new[] { "line1", "line2", "line3" })]
    [InlineData("line1\nline2\nline3", new[] { "line1", "line2", "line3" })]
    [InlineData(" line1 \r\n line2 \r\n line3 ", new[] { "line1", "line2", "line3" })]
    [InlineData("", new string[] {""})]
    [InlineData("singleline", new[] { "singleline" })]
    public void GetLinesFromText_VariousInputs_ReturnsExpectedResults(string text, string[] expected)
    {
        // Act
        var result = _service.GetLinesFromText(text);

        // Assert
        Assert.Equal(expected, result);
    }
}