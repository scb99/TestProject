using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor3.Services.ExpiredMemberships;
using Moq;

namespace DataRetrieval;

public class RetrieveExpiredMembershipsDataServiceTests
{
    private readonly Mock<IGetExpiredMemberships> mockGetExpiredMemberships = new(); 
    private readonly Mock<IGetMembersInGoodStanding> mockGetMembersInGoodStanding = new();
    private readonly RetrieveExpiredMembershipsDataService service;

    public RetrieveExpiredMembershipsDataServiceTests()
    {
        service = new RetrieveExpiredMembershipsDataService(mockGetExpiredMemberships.Object, mockGetMembersInGoodStanding.Object);
    }

    [Fact]
    public async Task GetExpiredMembershipsAsync_FiltersOutGoodStandingAndDeceasedMembers()
    {
        // Arrange
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2020, 12, 31);
        var goodStandingMembers = new List<UserIDsEntity>
        {
            new() { ID = 1 },
            new() { ID = 2 }
        };
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() { ID = 1, Deceased = "No" }, // Should be filtered out (good standing)
            new() { ID = 3, Deceased = "Yes" }, // Should be filtered out (deceased)
            new() { ID = 4, Deceased = "No" }  // Should remain
        };

        mockGetMembersInGoodStanding.Setup(m => m.GetMembersInGoodStandingSPAsync()).ReturnsAsync(goodStandingMembers);
        mockGetExpiredMemberships.Setup(m => m.GetExpiredMembershipsSPAsync(startDate, endDate)).ReturnsAsync(expiredMemberships);

        // Act
        var result = await service.RetrieveExpiredMembershipsAsync(startDate, endDate);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, r => r.ID == 4);
    }
}