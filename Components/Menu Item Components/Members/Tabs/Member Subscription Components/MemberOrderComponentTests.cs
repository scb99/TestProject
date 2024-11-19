using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;

namespace MenuItemComponents;

public class MemberOrderComponentTests
{
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly Mock<IRepositoryOrdersByID> _mockGetOrdersByID;
    private readonly MemberOrderComponent _component;

    public MemberOrderComponentTests()
    {
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _mockGetOrdersByID = new Mock<IRepositoryOrdersByID>();

        _component = new MemberOrderComponent();

        _component.SetPrivatePropertyValue("MemberNameService", _mockMemberNameService.Object);
        _component.SetPrivatePropertyValue("OrdersByIDRepository", _mockGetOrdersByID.Object);
    }

    [Fact]
    public async Task OnParametersSetAsync_SelectedIDIsNotZero_FetchesOrderDataAndGeneratesTitle()
    {
        // Arrange
        int selectedID = 1;
        _component.SetPublicPropertyValue("SelectedID", selectedID);
        var orders = new List<OrderEntity> { new(), new() };
        _mockGetOrdersByID.Setup(service => service.GetOrdersByIDAsync(selectedID)).ReturnsAsync(orders);
        _mockMemberNameService.Setup(service => service.MemberName).Returns("John Doe");

        // Act
        await typeof(MemberOrderComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(orders, _component.GetPrivatePropertyValue<List<OrderEntity>>("OrderEntitiesBDP"));
        Assert.Equal("2 Order entries for John Doe", _component.GetPrivatePropertyValue<string>("TitleBDP"));
    }

    [Fact]
    public async Task OnParametersSetAsync_SelectedIDIsZero_DoesNotFetchOrderDataOrGenerateTitle()
    {
        // Arrange
        _component.SetPublicPropertyValue("SelectedID", 0);

        // Act
        await typeof(MemberOrderComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        _mockGetOrdersByID.Verify(service => service.GetOrdersByIDAsync(It.IsAny<int>()), Times.Never);
        Assert.Empty(_component.GetPrivatePropertyValue<List<OrderEntity>>("OrderEntitiesBDP"));
        Assert.Null(_component.GetPrivatePropertyValue<string>("TitleBDP"));
    }

    [Fact]
    public async Task FetchOrderDataAsync_ValidID_FetchesOrderData()
    {
        // Arrange
        int selectedID = 1;
        var orders = new List<OrderEntity> { new(), new() };
        _mockGetOrdersByID.Setup(service => service.GetOrdersByIDAsync(selectedID)).ReturnsAsync(orders);

        // Act
        await typeof(MemberOrderComponent).InvokeAsync("FetchOrderDataAsync", _component, selectedID);

        // Assert
        Assert.Equal(orders, _component.GetPrivatePropertyValue<List<OrderEntity>>("OrderEntitiesBDP"));
    }

    [Fact]
    public void GenerateTitle_GeneratesCorrectTitle()
    {
        // Arrange
        var orders = new List<OrderEntity> { new() };
        _component.SetPrivatePropertyValue<List<OrderEntity>>("OrderEntitiesBDP", orders);
        _mockMemberNameService.Setup(service => service.MemberName).Returns("John Doe");

        // Act
        typeof(MemberOrderComponent).Invoke("GenerateTitle", _component);

        // Assert
        Assert.Equal("1 Order entry for John Doe", _component.GetPrivatePropertyValue<string>("TitleBDP"));
    }
}