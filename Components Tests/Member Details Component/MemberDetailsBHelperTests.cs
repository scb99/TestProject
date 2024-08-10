using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsBHelperTests
{
    [Fact]
    public void GetDisplayNames_ReturnsCorrectDisplayNames()
    {
        // Act
        var displayNames = MemberDetailsBHelper.GetDisplayNames();

        // Assert
        Assert.Equal(15, displayNames.Length);
        Assert.Contains("First Name", displayNames);
        Assert.Contains("Last Name", displayNames);
    }

    [Fact]
    public void GetMemberDetailEntities_ReturnsCorrectEntities()
    {
        // Arrange
        var displayNames = new[] { "First Name", "Last Name" };
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" }
        };

        var mockService = new Mock<ICrossCuttingMemberDetailsService>();
        mockService.Setup(s => s.MemberDetailEntities).Returns(memberDetailEntities);

        // Act
        var result = MemberDetailsBHelper.GetMemberDetailEntities(displayNames, mockService.Object);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("John", result[0].Value);
        Assert.Equal("Doe", result[1].Value);
    }

    [Fact]
    public void GetMemberName_ReturnsCorrectName()
    {
        // Arrange
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" }
        };

        // Act
        var memberName = MemberDetailsBHelper.GetMemberName(memberDetailEntities);

        // Assert
        Assert.Equal("Doe John", memberName);
    }

    [Fact]
    public void SkipInitialDetails_SkipsFirstTwoDetails()
    {
        // Arrange
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" },
            new() { DisplayName = "Email Privacy", Value = "Private" },
            new() { DisplayName = "Skill Rating", Value = "Expert" }
        };

        // Act
        var result = MemberDetailsBHelper.SkipInitialDetails(memberDetailEntities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Email Privacy", result[0].DisplayName);
        Assert.Equal("Skill Rating", result[1].DisplayName);
    }
}