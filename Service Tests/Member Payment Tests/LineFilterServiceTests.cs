using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;

namespace MemberPayment
{
    public class LineFilterServiceTests
    {
        private readonly ILineFilterService _lineFilterService;

        public LineFilterServiceTests()
        {
            _lineFilterService = new LineFilterService();
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("This line contains $", true)]
        [InlineData("This line contains renewal", true)]
        [InlineData("This line is valid", false)]
        public void ShouldIgnoreLine_VariousInputs_ReturnsExpectedResults(string line, bool expected)
        {
            // Act
            var result = _lineFilterService.ShouldIgnoreLine(line);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}