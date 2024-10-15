using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsCHelperTests
{
    [Fact]
    public void GetDisplayNames_ReturnsCorrectDisplayNames()
    {
        // Act
        var displayNames = MemberDetailsCHelper.GetDisplayNames();

        // Assert
        var expectedDisplayNames = new[]
        {
            "Last Name", "First Name", "Comments", "Skills/Hobbies", "STPC Note", "Reminder Sent"
        };
        Assert.Equal(expectedDisplayNames, displayNames);
    }

    [Fact]
    public void GetMemberDetailEntities_ReturnsCorrectEntities()
    {
        // Arrange
        var displayNames = new[]
        {
            "Last Name", "First Name", "Comments", "Skills/Hobbies", "STPC Note", "Reminder Sent"
        };
        var memberDetails = new List<MemberDetailEntity>
        {
            new() { DisplayName = "Last Name", Value = "Doe" },
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Comments", Value = "Test Comment" },
            new() { DisplayName = "Skills/Hobbies", Value = "Coding" },
            new() { DisplayName = "STPC Note", Value = "Note" },
            new() { DisplayName = "Reminder Sent", Value = "Yes" }
        };
        var mockDetailsService = new Mock<ICrossCuttingMemberDetailsService>();
        mockDetailsService.Setup(s => s.MemberDetailEntities).Returns(memberDetails);

        // Act
        var result = MemberDetailsCHelper.GetMemberDetailEntities(displayNames, mockDetailsService.Object);

        // Assert
        Assert.Equal(memberDetails, result);
    }

    [Fact]
    public void GetMemberName_ReturnsCorrectMemberName()
    {
        // Arrange
        var memberDetails = new List<MemberDetailEntity>
        {
            new() { DisplayName = "Last Name", Value = "Doe" },
            new() { DisplayName = "First Name", Value = "John" }
        };

        // Act
        var memberName = MemberDetailsCHelper.GetMemberName(memberDetails);

        // Assert
        Assert.Equal("John Doe", memberName);
    }

    [Fact]
    public void SkipInitialDetails_SkipsFirstTwoDetails()
    {
        // Arrange
        var memberDetails = new List<MemberDetailEntity>
        {
            new() { DisplayName = "Last Name", Value = "Doe" },
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Comments", Value = "Test Comment" },
            new() { DisplayName = "Skills/Hobbies", Value = "Coding" },
            new() { DisplayName = "STPC Note", Value = "Note" },
            new() { DisplayName = "Reminder Sent", Value = "Yes" }
        };

        // Act
        var result = MemberDetailsCHelper.SkipInitialDetails(memberDetails);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal("Test Comment", result[0].Value);
        Assert.Equal("Coding", result[1].Value);
        Assert.Equal("Note", result[2].Value);
        Assert.Equal("Yes", result[3].Value);
    }
}