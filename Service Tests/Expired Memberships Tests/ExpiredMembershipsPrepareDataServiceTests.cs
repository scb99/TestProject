using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace ExpiredMemberships;

public class ExpiredMembershipsPrepareDataServiceTests
{
    private readonly ExpiredMembershipsPrepareDataService _service;
    private readonly Mock<IRepositoryMemberDetail> _mockMemberDetailRepository;

    public ExpiredMembershipsPrepareDataServiceTests()
    {
        _mockMemberDetailRepository = new Mock<IRepositoryMemberDetail>();
        _service = new ExpiredMembershipsPrepareDataService(_mockMemberDetailRepository.Object);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_ValidData_PreparesCorrectly()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() 
            {
                ID = 1,
                Name = "John Doe",
                ExpirationNotice = "2023-12-31",
                Status = 1,
                RenewalDate = "2023-12-31"
            }
        };
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        _mockMemberDetailRepository
            .Setup(repo => repo.CreateMemberDetailDateTimeAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.True(result.Count == 3);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("12/31/2023", result[0].ExpirationNotice);
        Assert.Equal("Yes", result[0].Updated);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_ContainsHashInName_SkipsMember()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() 
            {
                ID = 1,
                Name = "#John Doe",
                ExpirationNotice = "2023-12-31",
                Status = 1,
                RenewalDate = "2023-12-31"
            }
        };
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.True(result.Count == 2);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_InvalidExpirationNotice_SetsEmptyString()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() 
            {
                ID = 1,
                Name = "John Doe",
                ExpirationNotice = "invalid date",
                Status = 1,
                RenewalDate = "2023-12-31"
            }
        };
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        _mockMemberDetailRepository
            .Setup(repo => repo.CreateMemberDetailDateTimeAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.True(result.Count == 3);
        Assert.Equal(string.Empty, result[0].ExpirationNotice);
    }
}