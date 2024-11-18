using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using ExtensionMethods;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace MenuItemComponents;

public class MemberPaymentHistoryComponentTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingMemberDetailsService> _mockMemberDetailsService;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly Mock<ICrossCuttingPaymentToMembershipIDService> _mockPaymentToMembershipIDService;
    private readonly Mock<IMemberPaymentHistoryOnActionService> _mockMemberPaymentHistoryOnActionService;
    private readonly Mock<IMemberPaymentHistoryTitleUpdater> _mockMemberPaymentHistoryTitleUpdater;
    private readonly Mock<IRepositoryPayementHistory> _mockPaymentHistoryDetailsRepository;
    private readonly MemberPaymentHistoryComponent _component;

    public MemberPaymentHistoryComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockMemberDetailsService = new Mock<ICrossCuttingMemberDetailsService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _mockPaymentToMembershipIDService = new Mock<ICrossCuttingPaymentToMembershipIDService>();
        _mockMemberPaymentHistoryOnActionService = new Mock<IMemberPaymentHistoryOnActionService>();
        _mockMemberPaymentHistoryTitleUpdater = new Mock<IMemberPaymentHistoryTitleUpdater>();
        _mockPaymentHistoryDetailsRepository = new Mock<IRepositoryPayementHistory>();

        _component = new MemberPaymentHistoryComponent();

        _component.SetPrivatePropertyValue("Show", _mockShow.Object);
        _component.SetPrivatePropertyValue("DBOperationService", _mockDBOperationService.Object);
        _component.SetPrivatePropertyValue("MemberDetailsService", _mockMemberDetailsService.Object);
        _component.SetPrivatePropertyValue("MemberNameService", _mockMemberNameService.Object);
        _component.SetPrivatePropertyValue("PaymentToMembershipIDService", _mockPaymentToMembershipIDService.Object);
        _component.SetPrivatePropertyValue("MemberPaymentHistoryOnActionService", _mockMemberPaymentHistoryOnActionService.Object);
        _component.SetPrivatePropertyValue("MemberPaymentHistoryTitleUpdater", _mockMemberPaymentHistoryTitleUpdater.Object);
        _component.SetPrivatePropertyValue("PaymentHistoryDetailsRepository", _mockPaymentHistoryDetailsRepository.Object);
    }

    [Fact]
    public async Task OnParametersSetAsync_LoadsPaymentHistoryDetails_WhenSelectedIDIsNotZero()
    {
        // Arrange
        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity> { new() };
        _mockPaymentHistoryDetailsRepository.Setup(repo => repo.GetAllByIDAsync(It.IsAny<int>())).ReturnsAsync(paymentHistoryDetails);
        _mockPaymentToMembershipIDService.Setup(service => service.GetPaymentToMembershipIDDictionaryAsync()).ReturnsAsync(new Dictionary<string, int>());
        _component.SetPublicPropertyValue<int>("SelectedID", 1);

        // Act
        await typeof(MemberPaymentHistoryComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        Assert.Equal(paymentHistoryDetails, _component.GetPrivatePropertyValue<List<PaymentHistoryDetailEntity>>("PaymentHistoryDetailEntitiesBDP"));
        Assert.Equal(paymentHistoryDetails.Count, _component.GetPrivateMemberValue<int>("_totalRowCount"));
    }

    [Fact]
    public async Task OnParametersSetAsync_DoesNotLoadPaymentHistoryDetails_WhenSelectedIDIsZero()
    {
        // Arrange
        _component.SetPrivatePropertyValue<int>("SelectedID", 0);

        // Act
        await typeof(MemberPaymentHistoryComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        _mockPaymentHistoryDetailsRepository.Verify(repo => repo.GetAllByIDAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task OnToolBarClickAsync_CancelsAndShowsAlert_WhenSelectedIDIsZero()
    {
        // Arrange
        _component.SetPrivatePropertyValue<int>("SelectedID", 0);
        var clickEventArgs = new ClickEventArgs { Cancel = false };

        // Act
        await typeof(MemberPaymentHistoryComponent).InvokeAsync("OnToolBarClickAsync", _component, clickEventArgs);

        // Assert
        Assert.True(clickEventArgs.Cancel);
        _mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync("Please select a member first!"), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_UpdatesTitleAndRowCount()
    {
        // Arrange
        _component.SetPrivatePropertyValue<int>("SelectedID", 1);
        var actionEventArgs = new ActionEventArgs<PaymentHistoryDetailEntity>();
        var updatedTitle = "Updated Title";
        var updatedRowCount = 10;

        _mockMemberPaymentHistoryOnActionService.Setup(service => service.OnActionBeginAsync(actionEventArgs, 
            _component.SelectedID, 
            _component._paymentToMembershipId, 
            _component.GetPrivatePropertyValue<List<PaymentHistoryDetailEntity>>("PaymentHistoryDetailEntitiesBDP"), 
            _component.GetPrivatePropertyValue<string>("TitleBDP"), 
            _component.GetPrivatePropertyValue<int>("._totalRowCount")))
            .ReturnsAsync(Tuple.Create(updatedTitle, updatedRowCount));

        // Act
        await typeof(MemberPaymentHistoryComponent).InvokeAsync("OnActionBeginAsync", _component, actionEventArgs);

        // Assert
        Assert.Equal(updatedTitle, _component.GetPrivatePropertyValue<string>("TitleBDP"));
        Assert.Equal(updatedRowCount, _component.GetPrivateMemberValue<int>("_totalRowCount"));
    }
}