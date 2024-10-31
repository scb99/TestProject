using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberPaymentHistoryTabItemComponentTests
{
    [Fact]
    public void MemberPaymentHistoryTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var component = new MemberPaymentHistoryTabItemComponent
        {
            MemberIDService = mockMemberIDService.Object
        };

        // Act
        var memberIDServiceField = component.GetType().GetProperty("MemberIDService"); //, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var memberIDServiceValue = memberIDServiceField?.GetValue(component);

        // Assert
        Assert.NotNull(memberIDServiceValue);
        Assert.IsAssignableFrom<ICrossCuttingMemberIDService>(memberIDServiceValue);
        Assert.Equal(mockMemberIDService.Object, memberIDServiceValue);
    }
}