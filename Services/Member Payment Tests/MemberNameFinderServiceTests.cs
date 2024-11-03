using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MemberPayment;

public class MemberNameFinderServiceTests
{
    private readonly Mock<ICrossCuttingAllMembersInDBService> _allMembersInDBServiceMock;
    private readonly MemberNameFinderService _service;

    public MemberNameFinderServiceTests()
    {
        _allMembersInDBServiceMock = new Mock<ICrossCuttingAllMembersInDBService>();
        _service = new MemberNameFinderService();
    }

    [Fact]
    public void FindName_ValidId_ReturnsCorrectName()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { ID = 1, LastName = "Doe", FirstName = "John" },
            new MemberEntity { ID = 2, LastName = "Smith", FirstName = "Jane" }
        };
        _allMembersInDBServiceMock.Setup(m => m.AllMembersInDB).Returns(members);

        // Act
        var result = _service.FindName(1, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.Equal(("Doe", "John"), result);
    }

    [Fact]
    public void FindName_InvalidId_ReturnsEmptyStrings()
    {
        // Arrange
        var members = new List<MemberEntity>
        {
            new MemberEntity { ID = 1, LastName = "Doe", FirstName = "John" },
            new MemberEntity { ID = 2, LastName = "Smith", FirstName = "Jane" }
        };
        _allMembersInDBServiceMock.Setup(m => m.AllMembersInDB).Returns(members);

        // Act
        var result = _service.FindName(3, _allMembersInDBServiceMock.Object);

        // Assert
        Assert.Equal((string.Empty, string.Empty), result);
    }
}