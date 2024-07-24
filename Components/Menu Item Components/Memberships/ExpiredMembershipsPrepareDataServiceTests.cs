using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace MenuItemComponents;

public class ExpiredMembershipsPrepareDataServiceTests
{
    private readonly Mock<IDataManager> _mockDataManager = new();
    private readonly ExpiredMembershipPrepareDataService _service;

    public ExpiredMembershipsPrepareDataServiceTests()
    {
        _service = new ExpiredMembershipPrepareDataService(_mockDataManager.Object);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_ExcludesMembersWithNameContainingHash()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() { Name = "John Doe" },
            new() { Name = "#Jane Doe" }
        };
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;

        _mockDataManager.Setup(dm => dm.CreateMemberDetailDateTimeSPAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);
        result.RemoveAll(m => m.Name.Contains('#'));

        // Assert
        Assert.Single(result);
        Assert.DoesNotContain(result, m => m.Name.Contains('#'));
    }

    [Fact]
    public async Task PrepareDataForExportAsync_AddsSummaryRows()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() { Name = "John Doe" }
        };
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;

        _mockDataManager.Setup(dm => dm.CreateMemberDetailDateTimeSPAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.Equal(3, result.Count); // 1 data row + 2 summary rows
        Assert.Contains(result, m => m.Name.StartsWith("#Total expired memberships"));
        Assert.Contains(result, m => m.Name.StartsWith("#Report prepared on"));
    }

    // Additional tests can be added here to cover more scenarios
}