using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace DataRetrieval;

public class RetrieveExpiredMembershipsDataServiceTests
{
    private readonly RetrieveExpiredMembershipsDataService _service;
    private readonly Mock<IRepositoryExpiredMemberships> _mockExpiredMembershipsRepository;
    private readonly Mock<IRepository<UserIDsEntity>> _mockUserIDsRepository;

    public RetrieveExpiredMembershipsDataServiceTests()
    {
        _mockExpiredMembershipsRepository = new Mock<IRepositoryExpiredMemberships>();
        _mockUserIDsRepository = new Mock<IRepository<UserIDsEntity>>();

        _service = new RetrieveExpiredMembershipsDataService(
            _mockExpiredMembershipsRepository.Object,
            _mockUserIDsRepository.Object
        );
    }

    [Fact]
    public async Task RetrieveExpiredMembershipsAsync_FiltersOutGoodStandingAndDeceasedMembers()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        var membersInGoodStanding = new List<UserIDsEntity>
        {
            new() { ID = 1 },
            new() { ID = 2 }
        };
        _mockUserIDsRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(membersInGoodStanding);

        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() { ID = 1, Deceased = "No" },
            new() { ID = 2, Deceased = "No" },
            new() { ID = 3, Deceased = "Yes" },
            new() { ID = 4, Deceased = "No" }
        };
        _mockExpiredMembershipsRepository
            .Setup(repo => repo.GetExpiredMembershipsAsync(startDate, endDate))
            .ReturnsAsync(expiredMemberships);

        // Act
        var result = await _service.RetrieveExpiredMembershipsAsync(startDate, endDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(4, result.First().ID);
    }
}