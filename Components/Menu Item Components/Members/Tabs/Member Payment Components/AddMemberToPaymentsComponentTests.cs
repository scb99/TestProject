using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class AddMemberToPaymentsComponentTests
{
    private readonly Mock<IMemberPaymentService> _memberPaymentServiceMock;
    private readonly AddMemberToPaymentsComponent _component;

    public AddMemberToPaymentsComponentTests()
    {
        _memberPaymentServiceMock = new Mock<IMemberPaymentService>();

        _component = new AddMemberToPaymentsComponent();

        _component.SetPrivatePropertyValue("MemberPaymentService", _memberPaymentServiceMock.Object);
    }

    [Fact]
    public void OnParametersSet_ShouldUpdateIsAddButtonDisabledBDP()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 1);

        // Act
        typeof(AddMemberToPaymentsComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("IsAddButtonDisabledBDP"));

        // Arrange
        _component.SetPublicPropertyValue<int>("SelectedID", 0);
            
        // Act
        typeof(AddMemberToPaymentsComponent).Invoke("OnParametersSet", _component);

        // Assert
        Assert.True(_component.GetPrivatePropertyValue<bool>("IsAddButtonDisabledBDP"));
    }

    [Fact]
    public async Task OnAddButtonClickedAsync_ShouldCallAddPaymentAsync()
    {
        // Arrange
        int selectedID = 1;
        _component.SetPublicPropertyValue<int>("SelectedID", selectedID);

        // Act
        await typeof(AddMemberToPaymentsComponent).InvokeAsync("OnAddButtonClickedAsync", _component);

        // Assert
        _memberPaymentServiceMock.Verify(service => service.AddPaymentAsync(selectedID), Times.Once);
    }
}