using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class MemberDetailsAComponentTests
{
    private readonly MemberDetailsAComponent _component;
    private readonly Mock<ICrossCuttingMemberDetailsBaseService> _mockBaseService;
    private readonly Mock<ICrossCuttingMemberDetailsService> _mockDetailsService;

    public MemberDetailsAComponentTests()
    {
        _mockBaseService = new Mock<ICrossCuttingMemberDetailsBaseService>();
        _mockDetailsService = new Mock<ICrossCuttingMemberDetailsService>();

        _component = new MemberDetailsAComponent();

        _component.SetPrivatePropertyValue("MemberDetailsBaseService", _mockBaseService.Object);
        _component.SetPrivatePropertyValue("MemberDetailsService", _mockDetailsService.Object);
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsZero_DoesNotLoadMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 0);

        // Act
        typeof(MemberDetailsAComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.Empty(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal(" No Selected Member", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsNotZero_LoadsMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 1);
        var displayNames = new[] { "First Name", "Last Name" };
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" }
        };

        _mockDetailsService.Setup(s => s.MemberDetailEntities).Returns(memberDetailEntities);

        // Act
        typeof(MemberDetailsAComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.NotNull(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal("John Doe", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
    }

    [Fact]
    public async Task OnActionBeginAsync_BeginEdit_SetsDisplayNameBDP()
    {
        // Arrange
        var arg = new ActionEventArgs<MemberDetailEntity>
        {
            RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit,
            Data = new MemberDetailEntity { DisplayName = "John Doe" }
        };

        _mockBaseService.Setup(s => s.OnActionBeginAsync(arg, It.IsAny<MemberDetailEntity>()))
                        .ReturnsAsync(new MemberDetailEntity());

        // Act
        await typeof(MemberDetailsAComponent).InvokeAsync("OnActionBeginAsync", _component, arg);

        // Assert
        Assert.Equal("John Doe", _component.GetPrivatePropertyValue<string>("DisplayNameBDP"));
    }

    [Fact]
    public async Task OnActionBeginAsync_NotBeginEdit_DoesNotSetDisplayNameBDP()
    {
        // Arrange
        var arg = new ActionEventArgs<MemberDetailEntity>
        {
            RequestType = Syncfusion.Blazor.Grids.Action.Save,
            Data = new MemberDetailEntity { DisplayName = "John Doe" }
        };

        _mockBaseService.Setup(s => s.OnActionBeginAsync(arg, It.IsAny<MemberDetailEntity>()))
                        .ReturnsAsync(new MemberDetailEntity());

        // Act
        await typeof(MemberDetailsAComponent).InvokeAsync("OnActionBeginAsync", _component, arg);

        // Assert
        Assert.Null(_component.GetPrivatePropertyValue<string>("DisplayNameBDP"));
    }
}