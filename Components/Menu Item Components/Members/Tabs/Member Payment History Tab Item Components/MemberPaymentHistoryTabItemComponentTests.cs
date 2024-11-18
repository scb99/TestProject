using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class MemberPaymentHistoryTabItemComponentTests
{
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly MemberPaymentHistoryTabItemComponent _component;

    public MemberPaymentHistoryTabItemComponentTests()
    {
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        _component = new MemberPaymentHistoryTabItemComponent();

        _component.SetPrivatePropertyValue("MemberIDService", _mockMemberIDService.Object);
    }

    [Fact]
    public void MemberPaymentHistoryTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange and Act
        var injectedService = _component.GetPrivatePropertyValue<ICrossCuttingMemberIDService>("MemberIDService"); // as ICrossCuttingMemberIDService;

        // Assert
        Assert.NotNull(injectedService);
        Assert.IsAssignableFrom<ICrossCuttingMemberIDService>(injectedService);
        Assert.Equal(_mockMemberIDService.Object, injectedService);
    }
}