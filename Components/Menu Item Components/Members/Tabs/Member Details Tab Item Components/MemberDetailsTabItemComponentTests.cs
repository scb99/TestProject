using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsTabItemComponentTests
{
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly MemberDetailsTabItemComponent _component;

    public MemberDetailsTabItemComponentTests()
    {
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        _component = new MemberDetailsTabItemComponent();

        _component.SetPrivatePropertyValue("MemberIDService", _mockMemberIDService.Object);
    }

    [Fact]
    public void MemberDetailsTabItemComponent_InjectsMemberIDService()
    {
        // Arrange
        //var serviceField = _component.GetType().GetField("<MemberIDService>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        //serviceField!.SetValue(_component, _mockMemberIDService.Object);

        // Arrange and Act
        var injectedService = _component.GetPrivatePropertyValue<ICrossCuttingMemberIDService>("MemberIDService"); // as ICrossCuttingMemberIDService;

        // Assert
        //var injectedService = serviceField.GetValue(_component) as ICrossCuttingMemberIDService;
        Assert.NotNull(injectedService);
        Assert.Equal(_mockMemberIDService.Object, injectedService);
    }

    // Additional tests can be added here to verify other behaviors of the component
}