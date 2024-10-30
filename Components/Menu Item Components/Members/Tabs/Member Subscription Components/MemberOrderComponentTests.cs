using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberOrderComponentTests
{
    private readonly Mock<ICrossCuttingMemberNameService> mockMemberNameService;
    private readonly Mock<IRepositoryOrdersByID> mockGetOrdersByID;
    private readonly MemberOrderComponent memberOrderComponent;

    public MemberOrderComponentTests()
    {
        mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        mockGetOrdersByID = new Mock<IRepositoryOrdersByID>();
        memberOrderComponent = new MemberOrderComponent
        {
            MemberNameService = mockMemberNameService.Object,
            OrdersByIDRepository = mockGetOrdersByID.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_SelectedIDIsNotZero_FetchesOrderDataAndGeneratesTitle()
    {
        // Arrange
        int selectedID = 1;
        memberOrderComponent.Initialize(selectedID);
        var orders = new List<OrderEntity> { new(), new() };
        mockGetOrdersByID.Setup(service => service.GetOrdersByIDAsync(selectedID)).ReturnsAsync(orders);
        mockMemberNameService.Setup(service => service.MemberName).Returns("John Doe");

        // Act
        await memberOrderComponent.OnParametersSet2Async();

        // Assert
        Assert.Equal(orders, memberOrderComponent.OrderEntitiesBDP);
        Assert.Equal("2 Order entries for John Doe", memberOrderComponent.TitleBDP);
    }

    [Fact]
    public async Task OnParametersSetAsync_SelectedIDIsZero_DoesNotFetchOrderDataOrGenerateTitle()
    {
        // Arrange
        memberOrderComponent.Initialize(0);

        // Act
        await memberOrderComponent.OnParametersSet2Async();

        // Assert
        mockGetOrdersByID.Verify(service => service.GetOrdersByIDAsync(It.IsAny<int>()), Times.Never);
        Assert.Empty(memberOrderComponent.OrderEntitiesBDP);
        Assert.Null(memberOrderComponent.TitleBDP);
    }

    [Fact]
    public async Task FetchOrderDataAsync_ValidID_FetchesOrderData()
    {
        // Arrange
        int selectedID = 1;
        var orders = new List<OrderEntity> { new(), new() };
        mockGetOrdersByID.Setup(service => service.GetOrdersByIDAsync(selectedID)).ReturnsAsync(orders);

        // Act
        await memberOrderComponent.FetchOrderDataAsync(selectedID);

        // Assert
        Assert.Equal(orders, memberOrderComponent.OrderEntitiesBDP);
    }

    [Fact]
    public void GenerateTitle_GeneratesCorrectTitle()
    {
        // Arrange
        var orders = new List<OrderEntity> { new() };
        memberOrderComponent.OrderEntitiesBDP = orders;
        mockMemberNameService.Setup(service => service.MemberName).Returns("John Doe");

        // Act
        memberOrderComponent.GenerateTitle();

        // Assert
        Assert.Equal("1 Order entry for John Doe", memberOrderComponent.TitleBDP);
    }
}
