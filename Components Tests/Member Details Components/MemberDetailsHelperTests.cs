using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsHelperTests
{
    [Fact]
    public void GetDisplayNames_ReturnsCorrectDisplayNames()
    {
        // Act
        var displayNames = MemberDetailsHelper.GetDisplayNames();

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
        var result = MemberDetailsHelper.GetMemberDetailEntities(displayNames, mockService.Object);

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
        var memberName = MemberDetailsHelper.GetMemberName(memberDetailEntities);

        // Assert
        Assert.Equal("Doe John", memberName);
    }

    //[Fact]
    //public void CustomizeCell_SetsCorrectCellClass()
    //{
    //    // Arrange
    //    var args = new QueryCellInfoEventArgs<MemberDetailEntity>
    //    {
    //        Column = new Column { Field = "Value" },
    //        Data = new MemberDetailEntity { DisplayName = "First Name", Value = "John" },
    //        Cell = new Cell()
    //    };

    //    // Mock the validator
    //    var mockValidator = new Mock<MemberDetailsValidators>();
    //    mockValidator.Setup(v => v.IsMemberDetailsADataValid(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

    //    // Act
    //    MemberDetailsHelper.CustomizeCell(args);

    //    // Assert
    //    Assert.Contains("noerror", args.Cell.CssClass);
    //}
}