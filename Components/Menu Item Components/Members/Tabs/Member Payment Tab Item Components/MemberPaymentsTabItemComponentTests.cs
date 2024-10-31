using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberPaymentsTabItemComponentTests
{
    [Fact]
    public void MemberPaymentsTabItemComponent_ShouldInjectMemberIDService()
    {
        // Arrange
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var component = new MemberPaymentsTabItemComponent
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