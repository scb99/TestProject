using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents
{
    public class AddMemberToPaymentsComponentTests
    {
        private readonly Mock<IMemberPaymentService> _memberPaymentServiceMock;
        private readonly AddMemberToPaymentsComponent _component;

        public AddMemberToPaymentsComponentTests()
        {
            _memberPaymentServiceMock = new Mock<IMemberPaymentService>();
            _component = new AddMemberToPaymentsComponent
            {
                MemberPaymentService = _memberPaymentServiceMock.Object
            };
        }

        [Fact]
        public void OnParametersSet_ShouldUpdateIsAddButtonDisabledBDP()
        {
            // Arrange
            _component.SelectedID = 1;

            // Act
            _component.OnParametersSet2();

            // Assert
            Assert.False(_component.IsAddButtonDisabledBDP);

            // Arrange
            _component.SelectedID = 0;

            // Act
            _component.OnParametersSet2();

            // Assert
            Assert.True(_component.IsAddButtonDisabledBDP);
        }

        [Fact]
        public async Task OnAddButtonClickedAsync_ShouldCallAddPaymentAsync()
        {
            // Arrange
            int selectedID = 1;
            _component.SelectedID = selectedID;

            // Act
            await _component.OnAddButtonClickedAsync();

            // Assert
            _memberPaymentServiceMock.Verify(service => service.AddPaymentAsync(selectedID), Times.Once);
        }
    }
}