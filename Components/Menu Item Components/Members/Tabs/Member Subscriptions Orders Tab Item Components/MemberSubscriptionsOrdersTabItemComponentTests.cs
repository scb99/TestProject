using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberSubscriptionsOrdersTabItemComponentTests
{
    [Fact]
    public void MemberSubscriptionsOrdersTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var component = new MemberSubscriptionsOrdersTabItemComponent
        {
            MemberIDService = mockMemberIDService.Object
        };

        // Act
        var memberIDServiceField = component.GetType().GetProperty("MemberIDService"); //, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var memberIDServiceValue = memberIDServiceField?.GetValue(component);

        // Assert
        Assert.NotNull(memberIDServiceValue);
        Assert.Equal(mockMemberIDService.Object, memberIDServiceValue);
    }
}