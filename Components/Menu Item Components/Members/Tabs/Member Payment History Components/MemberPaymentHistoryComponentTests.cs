using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace MenuItemComponents;

public class MemberPaymentHistoryComponentTests
{
    [Fact]
    public async Task OnParametersSetAsync_LoadsPaymentHistoryDetails_WhenSelectedIDIsNotZero()
    {
        // Arrange
        var mockPaymentHistoryDetailsRepository = new Mock<IRepositoryPayementHistory>();
        var mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        var mockPaymentToMembershipIDService = new Mock<ICrossCuttingPaymentToMembershipIDService>();
        var mockMemberPaymentHistoryTitleUpdater = new Mock<IMemberPaymentHistoryTitleUpdater>();
        var mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        var mockMemberDetailsService = new Mock<ICrossCuttingMemberDetailsService>();

        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity> { new() };
        mockPaymentHistoryDetailsRepository.Setup(repo => repo.GetAllByIDAsync(It.IsAny<int>())).ReturnsAsync(paymentHistoryDetails);
        mockPaymentToMembershipIDService.Setup(service => service.GetPaymentToMembershipIDDictionaryAsync()).ReturnsAsync(new Dictionary<string, int>());

        var component = new MemberPaymentHistoryComponent
        {
            PaymentHistoryDetailsRepository = mockPaymentHistoryDetailsRepository.Object,
            DBOperationService = mockDBOperationService.Object,
            PaymentToMembershipIDService = mockPaymentToMembershipIDService.Object,
            MemberPaymentHistoryTitleUpdater = mockMemberPaymentHistoryTitleUpdater.Object,
            MemberNameService = mockMemberNameService.Object,
            MemberDetailsService = mockMemberDetailsService.Object
        };
        component.Initialize(1);

        // Act
        await component.OnParametersSet2Async();

        // Assert
        Assert.Equal(paymentHistoryDetails, component.PaymentHistoryDetailEntitiesBDP);
        Assert.Equal(paymentHistoryDetails.Count, component._totalRowCount);
    }

    [Fact]
    public async Task OnParametersSetAsync_DoesNotLoadPaymentHistoryDetails_WhenSelectedIDIsZero()
    {
        // Arrange
        var mockPaymentHistoryDetailsRepository = new Mock<IRepositoryPayementHistory>();
        var component = new MemberPaymentHistoryComponent
        {
            PaymentHistoryDetailsRepository = mockPaymentHistoryDetailsRepository.Object
        };
        component.Initialize(0);

        // Act
        await component.OnParametersSet2Async();

        // Assert
        mockPaymentHistoryDetailsRepository.Verify(repo => repo.GetAllByIDAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task OnToolBarClickAsync_CancelsAndShowsAlert_WhenSelectedIDIsZero()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var component = new MemberPaymentHistoryComponent
        {
            Show = mockShow.Object
        };
        component.Initialize(0);
        var clickEventArgs = new ClickEventArgs { Cancel = false };

        // Act
        await component.OnToolBarClickAsync(clickEventArgs);

        // Assert
        Assert.True(clickEventArgs.Cancel);
        mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync("Please select a member first!"), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_UpdatesTitleAndRowCount()
    {
        // Arrange
        var mockMemberPaymentHistoryOnActionService = new Mock<IMemberPaymentHistoryOnActionService>();
        var component = new MemberPaymentHistoryComponent
        {
            MemberPaymentHistoryOnActionService = mockMemberPaymentHistoryOnActionService.Object
        };
        component.Initialize(1);
        var actionEventArgs = new ActionEventArgs<PaymentHistoryDetailEntity>();
        var updatedTitle = "Updated Title";
        var updatedRowCount = 10;

        mockMemberPaymentHistoryOnActionService.Setup(service => service.OnActionBeginAsync(actionEventArgs, component.SelectedID, component._paymentToMembershipId, component.PaymentHistoryDetailEntitiesBDP, component.TitleBDP, component._totalRowCount))
            .ReturnsAsync(Tuple.Create(updatedTitle, updatedRowCount));

        // Act
        await component.OnActionBeginAsync(actionEventArgs);

        // Assert
        Assert.Equal(updatedTitle, component.TitleBDP);
        Assert.Equal(updatedRowCount, component._totalRowCount);
    }
}