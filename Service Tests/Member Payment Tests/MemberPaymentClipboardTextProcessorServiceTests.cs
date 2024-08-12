using DataAccess.Models;
using DBExplorerBlazor;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MemberPayment;

public class MemberPaymentClipboardTextProcessorServiceTests
{
    private readonly Mock<ILineFilterService> _lineFilterServiceMock;
    private readonly Mock<IPaymentEntityFactoryService> _paymentEntityFactoryServiceMock;
    private readonly Mock<ITextLineSplitterService> _textLineSplitterServiceMock;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _allMembersInDBServiceMock;
    private readonly MemberPaymentClipboardTextProcessorService _service;

    public MemberPaymentClipboardTextProcessorServiceTests()
    {
        _lineFilterServiceMock = new Mock<ILineFilterService>();
        _paymentEntityFactoryServiceMock = new Mock<IPaymentEntityFactoryService>();
        _textLineSplitterServiceMock = new Mock<ITextLineSplitterService>();
        _allMembersInDBServiceMock = new Mock<ICrossCuttingAllMembersInDBService>();

        _service = new MemberPaymentClipboardTextProcessorService(
            _lineFilterServiceMock.Object,
            _paymentEntityFactoryServiceMock.Object,
            _textLineSplitterServiceMock.Object
        );
    }

    [Fact]
    public void ProcessText_ValidText_ReturnsPaymentEntities()
    {
        // Arrange
        var textFromClipboard = "1 R 100\n2 D 200";
        var lines = new[] { "1 R 100", "2 D 200" };
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, LastName = "Doe", FirstName = "John", Description = "R", Amount = "100" },
            new() { ID = 2, LastName = "Smith", FirstName = "Jane", Description = "D", Amount = "200" }
        };

        _textLineSplitterServiceMock.Setup(m => m.GetLinesFromText(textFromClipboard)).Returns(lines);
        _lineFilterServiceMock.Setup(m => m.ShouldIgnoreLine(It.IsAny<string>())).Returns(false);
        _paymentEntityFactoryServiceMock.SetupSequence(m => m.TryCreatePaymentEntityFromLine(It.IsAny<string>(), _allMembersInDBServiceMock.Object))
            .Returns((true, paymentEntities[0]))
            .Returns((true, paymentEntities[1]));

        // Act
        var result = _service.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paymentEntities.Count, result.Count);
    }

    [Fact]
    public void ProcessText_InvalidText_ReturnsNull()
    {
        // Arrange
        var textFromClipboard = "invalid text";
        var lines = new[] { "invalid text" };

        _textLineSplitterServiceMock.Setup(m => m.GetLinesFromText(textFromClipboard)).Returns(lines);
        _lineFilterServiceMock.Setup(m => m.ShouldIgnoreLine(It.IsAny<string>())).Returns(false);
        _paymentEntityFactoryServiceMock.Setup(m => m.TryCreatePaymentEntityFromLine(It.IsAny<string>(), _allMembersInDBServiceMock.Object))
            .Returns((false, null));

        // Act
        var result = _service.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ProcessText_IgnoredLines_AreSkipped()
    {
        // Arrange
        var textFromClipboard = "1 R 100\n$ ignored line\nrenewal ignored line";
        var lines = new[] { "1 R 100", "$ ignored line", "renewal ignored line" };
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, LastName = "Doe", FirstName = "John", Description = "R", Amount = "100" }
        };

        _textLineSplitterServiceMock.Setup(m => m.GetLinesFromText(textFromClipboard)).Returns(lines);
        _lineFilterServiceMock.SetupSequence(m => m.ShouldIgnoreLine(It.IsAny<string>()))
            .Returns(false)
            .Returns(true)
            .Returns(true);
        _paymentEntityFactoryServiceMock.Setup(m => m.TryCreatePaymentEntityFromLine(It.IsAny<string>(), _allMembersInDBServiceMock.Object))
            .Returns((true, paymentEntities[0]));

        // Act
        var result = _service.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}