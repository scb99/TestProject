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
        // Arrange and Act
        var injectedService = _component.GetPrivatePropertyValue<ICrossCuttingMemberIDService>("MemberIDService"); // as ICrossCuttingMemberIDService;

        // Assert
        Assert.NotNull(injectedService);
        Assert.IsAssignableFrom<ICrossCuttingMemberIDService>(injectedService);
        Assert.Equal(_mockMemberIDService.Object, injectedService);
    }
}