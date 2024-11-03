using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class UpdateMemberServiceTests
{
    private readonly Mock<ICrossCuttingMemberIDService> mockMemberIDService;
    private readonly Mock<ICrossCuttingMemberNameService> mockMemberNameService;
    private readonly UpdateMemberService updateMemberService;

    public UpdateMemberServiceTests()
    {
        mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();

        updateMemberService = new UpdateMemberService(mockMemberIDService.Object, mockMemberNameService.Object);
    }

    [Fact]
    public void UpdateMemberServices_UpdatesMemberDetails_WhenMemberExists()
    {
        // Arrange
        var selectedMemberID = 1;
        var memberEntities = new List<MemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" },
            new() { ID = 2, FirstName = "Jane", LastName = "Smith" }
        };

        // Act
        updateMemberService.UpdateMemberServices(selectedMemberID, memberEntities);

        // Assert
        mockMemberIDService.VerifySet(service => service.MemberID = selectedMemberID, Times.Once);
        mockMemberNameService.VerifySet(service => service.MemberName = "John Doe", Times.Once);
        mockMemberNameService.VerifySet(service => service.MemberFirstName = "John", Times.Once);
        mockMemberNameService.VerifySet(service => service.MemberLastName = "Doe", Times.Once);
    }

    [Fact]
    public void UpdateMemberServices_DoesNotUpdateMemberDetails_WhenMemberDoesNotExist()
    {
        // Arrange
        var selectedMemberID = 3;
        var memberEntities = new List<MemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" },
            new() { ID = 2, FirstName = "Jane", LastName = "Smith" }
        };

        // Act
        updateMemberService.UpdateMemberServices(selectedMemberID, memberEntities);

        // Assert
        mockMemberIDService.VerifySet(service => service.MemberID = It.IsAny<int>(), Times.Never);
        mockMemberNameService.VerifySet(service => service.MemberName = It.IsAny<string>(), Times.Never);
        mockMemberNameService.VerifySet(service => service.MemberFirstName = It.IsAny<string>(), Times.Never);
        mockMemberNameService.VerifySet(service => service.MemberLastName = It.IsAny<string>(), Times.Never);
    }
}