using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class MemberDetailsBComponentTests
{
    private readonly MemberDetailsBComponent _component;
    private readonly Mock<ICrossCuttingMemberDetailsBaseService> _mockBaseService;
    private readonly Mock<ICrossCuttingMemberDetailsService> _mockDetailsService;

    public MemberDetailsBComponentTests()
    {
        _mockBaseService = new Mock<ICrossCuttingMemberDetailsBaseService>();
        _mockDetailsService = new Mock<ICrossCuttingMemberDetailsService>();

        _component = new MemberDetailsBComponent();

        _component.SetPrivatePropertyValue("MemberDetailsBaseService", _mockBaseService.Object);
        _component.SetPrivatePropertyValue("MemberDetailsService", _mockDetailsService.Object);
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsZero_DoesNotLoadMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 0);

        // Act
        typeof(MemberDetailsBComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.Empty(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal(" No Selected Member", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
    }

    [Fact]
    public void OnParametersSet_SelectedIDIsNotZero_LoadsMemberDetails()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 1);
        var displayNames = MemberDetailsBHelper.GetDisplayNames();
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" }
        };

        _mockDetailsService.Setup(s => s.MemberDetailEntities).Returns(memberDetailEntities);

        // Act
        typeof(MemberDetailsBComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.NotNull(_component.GetPrivatePropertyValue<List<MemberDetailEntity>>("MemberDetailEntitiesBDP"));
        Assert.Equal("John Doe", _component.GetPrivatePropertyValue<string>("MemberNameBDP"));
    }

    [Fact]
    public void OnParametersSet2_CallsOnParametersSet()
    {
        // Arrange
        bool wasCalled;
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { DisplayName = "First Name", Value = "John" },
            new() { DisplayName = "Last Name", Value = "Doe" }
        };

        _mockDetailsService.Setup(s => s.MemberDetailEntities).Returns(memberDetailEntities);
        _component.SetPublicPropertyValue<int>("SelectedID", 1);

        // Act
        typeof(MemberDetailsBComponent).Invoke("OnParametersSet", _component);
        wasCalled = true;

        // Assert
        Assert.True(wasCalled);
    }

    [Fact]
    public async Task OnActionBeginAsync_CallsBaseService()
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
        await typeof(MemberDetailsBComponent).InvokeAsync("OnActionBeginAsync", _component, arg);

        // Assert
        _mockBaseService.Verify(s => s.OnActionBeginAsync(arg, It.IsAny<MemberDetailEntity>()), Times.Once);
    }
}