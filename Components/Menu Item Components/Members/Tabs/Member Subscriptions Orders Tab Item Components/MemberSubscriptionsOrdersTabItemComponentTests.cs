using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class MemberSubscriptionsOrdersTabItemComponentTests
{
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly MemberSubscriptionsOrdersTabItemComponent _component;

    public MemberSubscriptionsOrdersTabItemComponentTests()
    {
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        _component = new MemberSubscriptionsOrdersTabItemComponent();

        _component.SetPrivatePropertyValue("MemberIDService", _mockMemberIDService.Object);
    }

    [Fact]
    public void MemberSubscriptionsOrdersTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange and Act
        var injectedService = _component.GetPrivatePropertyValue<ICrossCuttingMemberIDService>("MemberIDService"); // as ICrossCuttingMemberIDService;

        // Assert
        Assert.NotNull(injectedService);
        Assert.IsAssignableFrom<ICrossCuttingMemberIDService>(injectedService);
        Assert.Equal(_mockMemberIDService.Object, injectedService);
    }
}