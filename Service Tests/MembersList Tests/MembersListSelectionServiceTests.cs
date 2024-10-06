using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MembersListSelectionServiceTests
{
    [Fact]
    public async Task ProcessSelectedMemberAsync_CallsDependenciesCorrectly()
    {
        // Arrange
        var mockMemberDetailsService = new Mock<ICrossCuttingMemberDetailsService>();
        var mockUpdateMemberService = new Mock<ICrossCuttingUpdateMemberService>();
        var mockGetMemberDetails = new Mock<IRepositoryMemberDetail2>();
        var memberSelectionService = new MembersListSelectionService(mockGetMemberDetails.Object, mockMemberDetailsService.Object, mockUpdateMemberService.Object);

        int testMemberID = 1;
        var memberEntities = new List<MemberEntity>();
        var memberDetailEntities = new List<MemberDetailEntity>();
        mockGetMemberDetails.Setup(dm => dm.GetMemberDetailsAsync(testMemberID)).ReturnsAsync(memberDetailEntities);

        // Act
        await memberSelectionService.ProcessSelectedMemberAsync(testMemberID, memberEntities);

        // Assert
        mockGetMemberDetails.Verify(dm => dm.GetMemberDetailsAsync(testMemberID), Times.Once);
        mockMemberDetailsService.VerifySet(mds => mds.MemberDetailEntities = memberDetailEntities, Times.Once);
        mockUpdateMemberService.Verify(ums => ums.UpdateMemberServices(testMemberID, memberEntities), Times.Once);
    }
}