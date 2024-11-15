using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class MemberDetailsCComponentTests
{
    private readonly MemberDetailsCComponent _component;
    private readonly Mock<ICrossCuttingMemberDetailsBaseService> _mockBaseService;
    private readonly Mock<ICrossCuttingMemberDetailsService> _mockDetailsService;

    public MemberDetailsCComponentTests()
    {
        _mockBaseService = new Mock<ICrossCuttingMemberDetailsBaseService>();
        _mockDetailsService = new Mock<ICrossCuttingMemberDetailsService>();

        _component = new MemberDetailsCComponent();

        _component.SetPrivatePropertyValue("MemberDetailsBaseService", _mockBaseService.Object);
        _component.SetPrivatePropertyValue("MemberDetailsService", _mockDetailsService.Object);
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsZero_DoesNotLoadMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 0);

        // Act
        typeof(MemberDetailsCComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.Empty(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal(" No Selected Member", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsNotZero_LoadsMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 1);
        var memberDetails = new List<MemberDetailEntity>
        {
            new() { DisplayName = "Last Name", Value = "Doe" },
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Comments", Value = "Test Comment" },
            new() { DisplayName = "Skills/Hobbies", Value = "Coding" },
            new() { DisplayName = "STPC Note", Value = "Note" },
            new() { DisplayName = "Reminder Sent", Value = "Yes" }
        };
        _mockDetailsService.Setup(s => s.MemberDetailEntities).Returns(memberDetails);

        // Act
        typeof(MemberDetailsCComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.NotNull(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal("John Doe", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
        Assert.Equal(4, _component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP").Count); // Skipped first 2
    }

    [Fact]
    public async Task OnActionBeginAsync_CallsBaseService()
    {
        // Arrange
        var arg = new ActionEventArgs<MemberDetailEntity>();
        var clonedEntity = new MemberDetailEntity();
        _mockBaseService.Setup(s => s.OnActionBeginAsync(arg, null)).ReturnsAsync(clonedEntity);

        // Act
        await typeof(MemberDetailsCComponent).InvokeAsync("OnActionBeginAsync", _component, arg);

        // Assert
        _mockBaseService.Verify(s => s.OnActionBeginAsync(arg, null), Times.Once);
        Assert.Equal(clonedEntity, _component.GetPrivateMemberValue<MemberDetailEntity>("_clonedMemberDetailEntity"));
    }
}