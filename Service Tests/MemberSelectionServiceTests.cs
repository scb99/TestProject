using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberSelectionServiceTests
{
    private readonly Mock<IAllMembersInDBService> _allMembersInDBServiceMock = new();
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly Mock<ILoggerService> _loggerServiceMock = new();
    private readonly Mock<IMemberDetailsService> _memberDetailsServiceMock = new();
    private readonly Mock<IUpdateMemberService> _updateMemberServiceMock = new();
    private readonly MemberSelectionService _memberSelectionService;

    public MemberSelectionServiceTests()
    {
        _memberSelectionService = new MemberSelectionService(_allMembersInDBServiceMock.Object, _dataManagerMock.Object, _loggerServiceMock.Object, _memberDetailsServiceMock.Object, _updateMemberServiceMock.Object);
    }

    [Fact]
    public async Task ProcessSelectedMemberAsync_ProcessesMemberSuccessfully()
    {
        // Arrange
        int selectedMemberID = 1;
        var memberEntities = new List<MemberEntity> { new() { ID = selectedMemberID } };
        var memberDetailEntities = new List<MemberDetailEntity> { new() { /* Initialize properties as needed */ } };

        _dataManagerMock.Setup(dm => dm.GetMemberDetailsSPAsync(selectedMemberID)).ReturnsAsync(memberDetailEntities);
        _allMembersInDBServiceMock.Setup(am => am.MemberNameDictionary).Returns(new Dictionary<int, string> { { selectedMemberID, "Test Member" } });

        // Act
        await _memberSelectionService.ProcessSelectedMemberAsync(selectedMemberID, memberEntities);

        // Assert
        _memberDetailsServiceMock.VerifySet(mds => mds.MemberDetailEntities = It.IsAny<List<MemberDetailEntity>>(), Times.Once);
        _loggerServiceMock.Verify(ls => ls.LogResultAsync(It.IsAny<string>()), Times.Once);
        _updateMemberServiceMock.Verify(ums => ums.UpdateMemberServices(selectedMemberID, memberEntities), Times.Once);
    }

    [Fact]
    public async Task ProcessSelectedMemberAsync_LogsException_OnFailure()
    {
        // Arrange
        int selectedMemberID = 1;
        var memberEntities = new List<MemberEntity>();
        var exception = new Exception("Test exception");

        _dataManagerMock.Setup(dm => dm.GetMemberDetailsSPAsync(selectedMemberID)).ThrowsAsync(exception);

        // Act
        await _memberSelectionService.ProcessSelectedMemberAsync(selectedMemberID, memberEntities);

        // Assert
        _loggerServiceMock.Verify(ls => ls.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }
}
