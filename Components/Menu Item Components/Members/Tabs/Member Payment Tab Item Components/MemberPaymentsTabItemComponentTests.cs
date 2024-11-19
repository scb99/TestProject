using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class MemberPaymentsTabItemComponentTests
{
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly MemberPaymentsTabItemComponent _component;

    public MemberPaymentsTabItemComponentTests()
    {
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        _component = new MemberPaymentsTabItemComponent();

        _component.SetPrivatePropertyValue("MemberIDService", _mockMemberIDService.Object);
    }

    [Fact]
    public void MemberPaymentsTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange and Act
        var injectedService = _component.GetPrivatePropertyValue<ICrossCuttingMemberIDService>("MemberIDService"); // as ICrossCuttingMemberIDService;

        // Assert
        Assert.NotNull(injectedService);
        Assert.Equal(_mockMemberIDService.Object, injectedService);
    }
}