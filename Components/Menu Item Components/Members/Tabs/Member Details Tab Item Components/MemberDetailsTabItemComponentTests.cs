using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberDetailsTabItemComponentTests
{
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly MemberDetailsTabItemComponent _component;

    public MemberDetailsTabItemComponentTests()
    {
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        _component = new MemberDetailsTabItemComponent
        {
            MemberIDService = _mockMemberIDService.Object
        };
    }

    [Fact]
    public void MemberIDService_IsInjected()
    {
        // Arrange & Act
        var memberIDServiceField = _component.GetType().GetProperty("MemberIDService"); //, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var memberIDServiceValue = memberIDServiceField?.GetValue(_component);

        // Assert
        Assert.NotNull(memberIDServiceValue);
        Assert.IsAssignableFrom<ICrossCuttingMemberIDService>(memberIDServiceValue);
    }

    // Additional tests can be added here to verify other behaviors of the component
}