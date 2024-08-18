using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;

namespace MenuItemComponents;

public class MemberOrderComponentTests
{
    private readonly MemberOrderComponent _component;
    private readonly Mock<IDataManager> _mockDataManager;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;

    public MemberOrderComponentTests()
    {
        _mockDataManager = new Mock<IDataManager>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _component = new MemberOrderComponent
        {
            DataManager = _mockDataManager.Object,
            MemberNameService = _mockMemberNameService.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldFetchOrderDataAndGenerateTitle_WhenSelectedIDIsNotZero()
    {
        // Arrange
        _component.SelectedID = 1;
        var orders = new List<OrderEntity> { new() { OrderID = 1 } };
        _mockDataManager.Setup(dm => dm.GetOrdersByIDSPAsync(It.IsAny<int>())).ReturnsAsync(orders);
        _mockMemberNameService.Setup(mns => mns.MemberName).Returns("John Doe");

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(orders, _component.OrderEntitiesBDP);
        Assert.Equal("1 Order entry for John Doe", _component.TitleBDP);
    }

    [Fact]
    public async Task OnParametersSetAsync_ShouldNotFetchOrderDataOrGenerateTitle_WhenSelectedIDIsZero()
    {
        // Arrange
        _component.SelectedID = 0;

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _mockDataManager.Verify(dm => dm.GetOrdersByIDSPAsync(It.IsAny<int>()), Times.Never);
        Assert.Empty(_component.OrderEntitiesBDP);
        Assert.Null(_component.TitleBDP);
    }

    [Fact]
    public async Task FetchOrderDataAsync_ShouldFetchOrderData()
    {
        // Arrange
        var orders = new List<OrderEntity> { new() { OrderID = 1 } };
        _mockDataManager.Setup(dm => dm.GetOrdersByIDSPAsync(It.IsAny<int>())).ReturnsAsync(orders);

        // Act
        await _component.FetchOrderDataAsync(1);

        // Assert
        Assert.Equal(orders, _component.OrderEntitiesBDP);
    }

    [Fact]
    public void GenerateTitle_ShouldGenerateCorrectTitle()
    {
        // Arrange
        _component.OrderEntitiesBDP = new List<OrderEntity> { new() { OrderID = 1 } };
        _mockMemberNameService.Setup(mns => mns.MemberName).Returns("John Doe");

        // Act
        _component.GenerateTitle();

        // Assert
        Assert.Equal("1 Order entry for John Doe", _component.TitleBDP);
    }

    [Fact]
    public async Task OnParametersSet2Async_ShouldCallOnParametersSetAsync()
    {
        // Arrange
        var orders = new List<OrderEntity> { new() { OrderID = 1 } };
        _mockDataManager.Setup(dm => dm.GetOrdersByIDSPAsync(It.IsAny<int>())).ReturnsAsync(orders);
        _mockMemberNameService.Setup(mns => mns.MemberName).Returns("John Doe");
        _component.SelectedID = 1;

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(orders, _component.OrderEntitiesBDP);
        Assert.Equal("1 Order entry for John Doe", _component.TitleBDP);
    }
}