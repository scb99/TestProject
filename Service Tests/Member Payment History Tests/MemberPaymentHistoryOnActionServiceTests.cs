using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MemberPaymentHistory;

public class MemberPaymentHistoryOnActionServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<IDataManager> _mockDataManager;
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly MemberPaymentHistoryOnActionService _service;

    public MemberPaymentHistoryOnActionServiceTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockDataManager = new Mock<IDataManager>();
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();

        _service = new MemberPaymentHistoryOnActionService(
            _mockShow.Object,
            _mockAllMembersInDBService.Object,
            _mockDataManager.Object,
            _mockDBOperationService.Object,
            _mockLoggedInMemberService.Object,
            _mockLogger.Object,
            _mockMemberNameService.Object,
            _mockSystemTimeService.Object);
    }

    [Fact]
    public async Task OnActionBeginAsync_AddOperation_ShouldPrepareForAdd()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Add, Data = new PaymentHistoryDetailEntity() };
        var selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int>();
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        var titleBDP = "Title";
        var totalRowCount = 0;

        _mockSystemTimeService.Setup(s => s.Now).Returns(DateTime.Now);

        // Act
        var result = await _service.OnActionBeginAsync(arg, selectedID, paymentToMembershipId, paymentHistoryDetailEntitiesBDP, titleBDP, totalRowCount);

        // Assert
        Assert.Equal(0, arg.Data.ID);
        Assert.Equal(30.00, arg.Data.InitialPayment);
        Assert.Equal("30", arg.Data.InitialPaymentString);
        Assert.Equal(1, arg.Data.MembershipID);
        Assert.Equal("active", arg.Data.Status);
        Assert.Equal(selectedID, arg.Data.UserID);
    }

    [Fact]
    public async Task OnActionBeginAsync_DeleteOperation_ShouldHandleDelete()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Delete, Data = new PaymentHistoryDetailEntity { ID = 1, UserID = 1, MembershipID = 1, Status = "active", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1) } };
        var selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int>();
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        var titleBDP = "Title";
        var totalRowCount = 0;

        _mockLoggedInMemberService.Setup(s => s.MemberRole).Returns("SuperUser");
        _mockDataManager.Setup(d => d.DeletePaymentDetailAsync(It.IsAny<int>())).ReturnsAsync(true);
        _mockAllMembersInDBService.Setup(a => a.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "John Doe" } });

        // Act
        var result = await _service.OnActionBeginAsync(arg, selectedID, paymentToMembershipId, paymentHistoryDetailEntitiesBDP, titleBDP, totalRowCount);

        // Assert
        _mockDataManager.Verify(d => d.DeletePaymentDetailAsync(It.IsAny<int>()), Times.Once);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_SaveOperation_ShouldHandleSave()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = new PaymentHistoryDetailEntity { ID = 0, UserID = 1, InitialPaymentString = "30", MembershipID = 1, Status = "active", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1) } };
        var selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int> { { "30", 1 } };
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        var titleBDP = "Title";
        var totalRowCount = 0;

        _mockDataManager.Setup(d => d.CreatePaymentDetailSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(true);
        _mockDataManager.Setup(d => d.GetLastPaymentIDSPAsync(It.IsAny<int>())).ReturnsAsync(1);
        _mockAllMembersInDBService.Setup(a => a.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "John Doe" } });

        // Act
        var result = await _service.OnActionBeginAsync(arg, selectedID, paymentToMembershipId, paymentHistoryDetailEntitiesBDP, titleBDP, totalRowCount);

        // Assert
        _mockDataManager.Verify(d => d.CreatePaymentDetailSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }
}