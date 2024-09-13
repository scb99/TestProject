//using DataAccess;
//using DataAccess.Models;
//using DBExplorerBlazor3.Services.ExpiredMemberships;
//using Moq;

//namespace MenuItemComponents;

//public class ExpiredMembershipsLoadDataServiceTests
//{
//    private readonly Mock<IDataManager> mockDataManager = new();
//    private readonly ExpiredMembershipsLoadDataService service;

//    public ExpiredMembershipsLoadDataServiceTests()
//    {
//        service = new ExpiredMembershipsLoadDataService(mockDataManager.Object);
//    }

//    [Fact]
//    public async Task GetExpiredMembershipsAsync_FiltersOutGoodStandingAndDeceasedMembers()
//    {
//        // Arrange
//        var startDate = new DateTime(2020, 1, 1);
//        var endDate = new DateTime(2020, 12, 31);
//        var goodStandingMembers = new List<UserIDsEntity>
//        {
//            new() { ID = 1 },
//            new() { ID = 2 }
//        };
//        var expiredMemberships = new List<ExpiredMembershipsEntity>
//        {
//            new() { ID = 1, Deceased = "No" }, // Should be filtered out (good standing)
//            new() { ID = 3, Deceased = "Yes" }, // Should be filtered out (deceased)
//            new() { ID = 4, Deceased = "No" }  // Should remain
//        };

//        mockDataManager.Setup(m => m.GetMembersInGoodStandingSPAsync()).ReturnsAsync(goodStandingMembers);
//        mockDataManager.Setup(m => m.GetExpiredMembershipsSPAsync(startDate, endDate)).ReturnsAsync(expiredMemberships);

//        // Act
//        var result = await service.GetExpiredMembershipsAsync(startDate, endDate);

//        // Assert
//        Assert.Single(result);
//        Assert.Contains(result, r => r.ID == 4);
//    }
//}