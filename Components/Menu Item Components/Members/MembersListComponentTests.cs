using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace MenuItemComponents;

public class MembersListComponentTests
{
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<IRetrieveMembersListDataService> _mockRetrieveService;
    private readonly Mock<IMembersListComboBoxProcessingService> _mockComboBoxService;
    private readonly Mock<IMembersListSelectionService> _mockSelectionService;
    private readonly Mock<IMembersListTextBoxProcessingService> _mockTextBoxService;
    private readonly MembersListComponent _component;

    public MembersListComponentTests()
    {
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockRetrieveService = new Mock<IRetrieveMembersListDataService>();
        _mockComboBoxService = new Mock<IMembersListComboBoxProcessingService>();
        _mockSelectionService = new Mock<IMembersListSelectionService>();
        _mockTextBoxService = new Mock<IMembersListTextBoxProcessingService>();

        _component = new MembersListComponent
        {
            LoadingPanelService = _mockLoadingPanelService.Object,
            RetrieveMembersListDataService = _mockRetrieveService.Object,
            MembersListComboBoxProcessingService = _mockComboBoxService.Object,
            MembersListSelectionService = _mockSelectionService.Object,
            MembersListTextBoxProcessService = _mockTextBoxService.Object,
        };

        _component.Initialize("500px");
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsMemberEntitiesAndTitle()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveMembersAsync())
                            .ReturnsAsync(members);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(members, _component.MemberEntitiesToDisplayBDP);
        Assert.Equal($"{members.Count} member{(members.Count == 1 ? "" : "s")} displayed", _component.TitleBDP);
    }

    [Fact]
    public async Task OnComboBoxChangedAsync_UpdatesMemberEntitiesAndTitle()
    {
        // Arrange
        var comboBoxValue = "Filter by First Name";
        var members = new List<MemberEntity> { new() };
        _mockComboBoxService.Setup(service => service.ProcessComboBoxChangeAsync(comboBoxValue))
                            .ReturnsAsync(members);

        // Act
        await _component.OnComboBoxChangedAsync(new ChangeEventArgs { Value = comboBoxValue });

        // Assert
        Assert.Equal(comboBoxValue, _component.ComboBoxSelectionBDP);
        Assert.Equal(members, _component.MemberEntitiesToDisplayBDP);
        Assert.Equal($"{members.Count} member{(members.Count == 1 ? "" : "s")} displayed", _component.TitleBDP);
    }

    [Fact]
    public async Task OnTextBoxInputChangedAsync_UpdatesMemberEntitiesAndTitle()
    {
        // Arrange
        var textBoxValue = "Smith";
        var members = new List<MemberEntity> { new() };
        _mockTextBoxService.Setup(service => service.ProcessTextBoxInputChangeAsync(_component.ComboBoxSelectionBDP, textBoxValue))
                           .ReturnsAsync(members);

        // Act
        await _component.OnTextBoxInputChangedAsync(new InputEventArgs { Value = textBoxValue });

        // Assert
        Assert.Equal(textBoxValue, _component.TextBoxValueBDP);
        Assert.Equal(members, _component.MemberEntitiesToDisplayBDP);
        Assert.Equal($"{members.Count} member{(members.Count == 1 ? "" : "s")} displayed", _component.TitleBDP);
    }

    [Fact]
    public async Task OnSelectedRowChangedAsync_ProcessesSelectedMember()
    {
        // Arrange
        var member = new MemberEntity { ID = 1 };
        var members = new List<MemberEntity> { member };
        _component.MemberEntitiesToDisplayBDP = members;

        // Act
        await _component.OnSelectedRowChangedAsync(new RowSelectEventArgs<MemberEntity> { Data = member });

        // Assert
        _mockSelectionService.Verify(service => service.ProcessSelectedMemberAsync(member.ID, members), Times.Once);
    }

    [Fact]
    public async Task LoadMembersAndManageUIAsync_SetsLoadingStateCorrectly()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _mockRetrieveService.Setup(service => service.RetrieveMembersAsync())
                            .ReturnsAsync(members);

        // Act
        await _component.LoadMembersAndManageUIAsync();

        // Assert
        Assert.False(_component.LoadingBDP);
        Assert.Equal(members, _component.MemberEntitiesToDisplayBDP);
        Assert.Equal($"{members.Count} member{(members.Count == 1 ? "" : "s")} displayed", _component.TitleBDP);
    }
}