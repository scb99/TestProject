using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace MenuItemComponents;

public class MemberSubscriptionComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ISubscriptionDataService> _mockSubscriptionDataService;
    private readonly Mock<ISubscriptionGridActionHandlerService> _mockSubscriptionGridActionHandlerService;
    private readonly Mock<ISubscriptionTitleGenerationService> _mockSubscriptionTitleGenerationService;
    private readonly MemberSubscriptionComponent _component;

    public MemberSubscriptionComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockSubscriptionDataService = new Mock<ISubscriptionDataService>();
        _mockSubscriptionGridActionHandlerService = new Mock<ISubscriptionGridActionHandlerService>();
        _mockSubscriptionTitleGenerationService = new Mock<ISubscriptionTitleGenerationService>();

        _component = new MemberSubscriptionComponent();

        _component.SetPrivatePropertyValue("Show", _mockShow.Object);
        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("SubscriptionDataService", _mockSubscriptionDataService.Object);
        _component.SetPrivatePropertyValue("SubscriptionGridActionHandlerService", _mockSubscriptionGridActionHandlerService.Object);
        _component.SetPrivatePropertyValue("SubscriptionTitleGenerationService", _mockSubscriptionTitleGenerationService.Object);
    }

    [Fact]
    public async Task OnParametersSetAsync_SetsSubscriptionEntitiesAndTitle()
    {
        // Arrange
        var selectedID = 1;
        var subscriptionEntities = new List<SubscriptionEntity> { new() };
        var title = "Test Title";
        _mockSubscriptionDataService.Setup(service => service.FetchSubscriptionDataAsync(selectedID)).ReturnsAsync(subscriptionEntities);
        _mockSubscriptionTitleGenerationService.Setup(service => service.GenerateSubscriptionTitle(subscriptionEntities)).Returns(title);
        _component.SetPublicPropertyValue("SelectedID", selectedID);

        // Act
        await typeof(MemberSubscriptionComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(subscriptionEntities, _component.GetPrivatePropertyValue<List<SubscriptionEntity>>("SubscriptionEntitiesBDP"));
        Assert.Equal(title, _component.GetPrivatePropertyValue<string>("TitleBDP"));
    }

    [Fact]
    public async Task OnActionBeginAsync_RefreshesGridAndUpdatesStateWhenUpdateOccurs()
    {
        // Arrange
        var arg = new ActionEventArgs<SubscriptionEntity>();
        _mockSubscriptionGridActionHandlerService.Setup(service => service.HandleActionBeginAsync(arg)).ReturnsAsync(true);
        _mockExecute.Setup(service => service.ConditionalCode()).Returns(false);
        _component.SetPrivatePropertyValue<SfGrid<SubscriptionEntity>>("SubscriptionGrid", new SfGrid<SubscriptionEntity>());

        // Act
        await typeof(MemberSubscriptionComponent).InvokeAsync("OnActionBeginAsync", _component, arg);

        // Assert
        _mockSubscriptionGridActionHandlerService.Verify(service => service.HandleActionBeginAsync(arg), Times.Once);
        _mockExecute.Verify(service => service.ConditionalCode(), Times.Once);
    }

    [Fact]
    public async Task OnToolBarClickAsync_ShowsAlertWhenNoMemberSelected()
    {
        // Arrange
        var arg = new ClickEventArgs();
        _component.SetPublicPropertyValue("SelectedID", 0);

        // Act
        await typeof(MemberSubscriptionComponent).InvokeAsync("OnToolBarClickAsync", _component, arg);

        // Assert
        Assert.True(arg.Cancel);
        _mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync("Please select a member first!"), Times.Once);
    }
}