using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberSelectionServiceTests
{
    [Fact]
    public async Task ProcessSelectedMemberAsync_CallsDependenciesCorrectly()
    {
        // Arrange
        var mockDataManager = new Mock<IDataManager>();
        var mockMemberDetailsService = new Mock<IMemberDetailsService>();
        var mockUpdateMemberService = new Mock<IUpdateMemberService>();
        var memberSelectionService = new MemberSelectionService(mockDataManager.Object, mockMemberDetailsService.Object, mockUpdateMemberService.Object);

        int testMemberID = 1;
        var memberEntities = new List<MemberEntity>();
        var memberDetailEntities = new List<MemberDetailEntity>();
        mockDataManager.Setup(dm => dm.GetMemberDetailsSPAsync(testMemberID)).ReturnsAsync(memberDetailEntities);

        // Act
        await memberSelectionService.ProcessSelectedMemberAsync(testMemberID, memberEntities);

        // Assert
        mockDataManager.Verify(dm => dm.GetMemberDetailsSPAsync(testMemberID), Times.Once);
        mockMemberDetailsService.VerifySet(mds => mds.MemberDetailEntities = memberDetailEntities, Times.Once);
        mockUpdateMemberService.Verify(ums => ums.UpdateMemberServices(testMemberID, memberEntities), Times.Once);
    }
}