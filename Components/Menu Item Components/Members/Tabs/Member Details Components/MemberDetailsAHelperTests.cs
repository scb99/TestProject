using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsAHelperTests
{
    [Fact]
    public void GetDisplayNames_ReturnsCorrectDisplayNames()
    {
        // Act
        var displayNames = MemberDetailsAHelper.GetDisplayNames();

        // Assert
        Assert.Equal(16, displayNames.Length);
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
        var result = MemberDetailsAHelper.GetMemberDetailEntities(displayNames, mockService.Object);

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
        var memberName = MemberDetailsAHelper.GetMemberName(memberDetailEntities);

        // Assert
        Assert.Equal("Doe John", memberName);
    }
}