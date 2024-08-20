using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor3.Services.ExpiredMemberships;
using Moq;

namespace MenuItemComponents;

public class ExpiredMembershipsPrepareDataServiceTests
{
    private readonly Mock<ICreateMemberDetailDateTime> _mockCreateMemberDetailDateTime;
    private readonly ExpiredMembershipsPrepareDataService _service;

    public ExpiredMembershipsPrepareDataServiceTests()
    {
        _mockCreateMemberDetailDateTime = new Mock<ICreateMemberDetailDateTime>();
        _service = new ExpiredMembershipsPrepareDataService(_mockCreateMemberDetailDateTime.Object);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_ValidData_PreparesDataCorrectly()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() {
                ID = 1,
                Name = "John Doe",
                //Status = "Active",
                RenewalDate = "2023-01-01",
                ExpirationNotice = "2023-12-31"
            }
        };
        DateTime startDate = new(2023, 1, 1);
        DateTime endDate = new(2023, 12, 31);

        _mockCreateMemberDetailDateTime
            .Setup(s => s.CreateMemberDetailDateTimeSPAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // 1 member + 2 summary entries
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("Yes", result[0].Updated);
        Assert.Equal("#Total expired memberships (between 1/1/2023 and 12/31/2023) = 1.", result[1].Name);
        Assert.Equal($"#Report prepared on {DateTime.Now.Date.ToShortDateString()}.", result[2].Name);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_MemberNameContainsHash_SkipsMember()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() {
                ID = 1,
                Name = "#John Doe",
                //Status = "Active",
                RenewalDate = "2023-01-01",
                ExpirationNotice = "2023-12-31"
            }
        };
        DateTime startDate = new(2023, 1, 1);
        DateTime endDate = new(2023, 12, 31);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // 0 members + 2 summary entries
        Assert.Equal("#Total expired memberships (between 1/1/2023 and 12/31/2023) = 0.", result[0].Name);
        Assert.Equal($"#Report prepared on {DateTime.Now.Date.ToShortDateString()}.", result[1].Name);
    }

    [Fact]
    public async Task PrepareDataForExportAsync_InvalidExpirationNotice_SetsEmptyString()
    {
        // Arrange
        var expiredMemberships = new List<ExpiredMembershipsEntity>
        {
            new() {
                ID = 1,
                Name = "John Doe",
                //Status = "Active",
                RenewalDate = "2023-01-01",
                ExpirationNotice = "InvalidDate"
            }
        };
        DateTime startDate = new(2023, 1, 1);
        DateTime endDate = new(2023, 12, 31);

        _mockCreateMemberDetailDateTime
            .Setup(s => s.CreateMemberDetailDateTimeSPAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.PrepareDataForExportAsync(expiredMemberships, startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // 1 member + 2 summary entries
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal(string.Empty, result[0].ExpirationNotice);
    }
}