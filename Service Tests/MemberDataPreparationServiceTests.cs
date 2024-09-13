using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace Service;

public class MemberDataPreparationServiceTests
{
    private readonly MemberDataPreparationService _service = new();

    [Fact]
    public void FormatNewMembersTXT_WithValidData_FormatsCorrectly()
    {
        // Arrange
        var newMemberEntities = new List<NewMemberEntity>
        {
            new() { Name = "John Doe", Gender = "M", Rating = "A", HomePhone = "1234567890", EmailAddress = "john@example.com", Address1 = "123 Main St", City = "Anytown", State = "ST", Zip = "12345" },
            // Add more members as needed
        };
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 1, 31);
        var now = new DateTime(2023, 2, 1);

        // Act
        var result = _service.FormatNewMembersTXT(newMemberEntities, startDate, endDate, now);

        // Assert
        Assert.Contains("John Doe", result);
        Assert.Contains("Total number of new STPC members enrolled", result);
        Assert.Contains("Report prepared on", result);
    }

    [Fact]
    public void PrepareDataForExport_WithValidData_ProcessesCorrectly()
    {
        // Arrange
        var newMemberEntities = new List<NewMemberEntity>
        {
            new() { Name = "Jane Doe", IsMemberInGoodStanding = true, EndDate = DateTime.Now.AddDays(365) },
            // Add more members as needed
        };
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 1, 31);
        var now = new DateTime(2023, 2, 1);

        // Act
        var result = _service.PrepareDataForExport(newMemberEntities, startDate, endDate, now);

        // Assert
        Assert.Contains(result, member => member.Name.Contains("Total STPC new members"));
        Assert.Contains(result, member => member.Name.Contains("Total new members in range that are still STPC members"));
        Assert.Contains(result, member => member.Name.Contains("Report prepared on"));
    }
}