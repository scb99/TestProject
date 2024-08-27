using DataAccess;
using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MemberPaymentHistory;

public class MemberPaymentHistoryOnActionServiceTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingDBOperationService> _mockDBOperationService;
    private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly Mock<ICrossCuttingSystemTimeService> _mockSystemTimeService;
    private readonly Mock<ICreatePaymentDetail> _mockCreatePaymentDetail;
    private readonly Mock<IDeletePaymentDetail> _mockDeletePaymentDetail;
    private readonly Mock<IGetLastPaymentID> _mockGetLastPaymentID;
    private readonly Mock<IGetSubscriptionsByID> _mockGetSubscriptionsByID;
    private readonly Mock<IUpdateSubscriptionNextPaymentDate> _mockUpdateSubscriptionNextPaymentDate;
    private readonly Mock<IUpdatePaymentDetail> _mockUpdatePaymentDetail;

    private readonly MemberPaymentHistoryOnActionService _service;

    public MemberPaymentHistoryOnActionServiceTests()
    {
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockDBOperationService = new Mock<ICrossCuttingDBOperationService>();
        _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        _mockCreatePaymentDetail = new Mock<ICreatePaymentDetail>();
        _mockDeletePaymentDetail = new Mock<IDeletePaymentDetail>();
        _mockGetLastPaymentID = new Mock<IGetLastPaymentID>();
        _mockGetSubscriptionsByID = new Mock<IGetSubscriptionsByID>();
        _mockUpdateSubscriptionNextPaymentDate = new Mock<IUpdateSubscriptionNextPaymentDate>();
        _mockUpdatePaymentDetail = new Mock<IUpdatePaymentDetail>();

        _service = new MemberPaymentHistoryOnActionService(
            _mockAlertService.Object,
            _mockAllMembersInDBService.Object,
            _mockDBOperationService.Object,
            _mockLoggedInMemberService.Object,
            _mockLogger.Object,
            _mockMemberNameService.Object,
            _mockSystemTimeService.Object,
            _mockCreatePaymentDetail.Object,
            _mockDeletePaymentDetail.Object,
            _mockGetLastPaymentID.Object,
            _mockGetSubscriptionsByID.Object,
            _mockUpdateSubscriptionNextPaymentDate.Object,
            _mockUpdatePaymentDetail.Object);
    }

    [Fact]
    public async Task OnActionBeginAsync_AddOperation_PreparesForAdd()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Add, Data = new PaymentHistoryDetailEntity() };
        int selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int>();
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        string titleBDP = "Test Title";
        int totalRowCount = 0;

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
    public async Task OnActionBeginAsync_DeleteOperation_DeletesRecord()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Delete, Data = new PaymentHistoryDetailEntity { ID = 1, UserID = 1, MembershipID = 1, Status = "active", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1) } };
        int selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int>();
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        string titleBDP = "Test Title";
        int totalRowCount = 0;

        _mockLoggedInMemberService.Setup(s => s.MemberRole).Returns("SuperUser");
        _mockDeletePaymentDetail.Setup(d => d.DeletePaymentDetailAsync(It.IsAny<int>())).ReturnsAsync(true);
        //_mockDataManager.Setup(d => d.DeletePaymentDetailAsync(It.IsAny<int>())).ReturnsAsync(true);
        _mockAllMembersInDBService.Setup(s => s.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Test User" } });

        // Act
        var result = await _service.OnActionBeginAsync(arg, selectedID, paymentToMembershipId, paymentHistoryDetailEntitiesBDP, titleBDP, totalRowCount);

        // Assert
        _mockDeletePaymentDetail.Verify(d => d.DeletePaymentDetailAsync(It.IsAny<int>()), Times.Once);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnActionBeginAsync_SaveOperation_CreatesRecord()
    {
        // Arrange
        var arg = new ActionEventArgs<PaymentHistoryDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = new PaymentHistoryDetailEntity { ID = 0, UserID = 1, MembershipID = 1, Status = "active", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), InitialPaymentString = "30" } };
        int selectedID = 1;
        var paymentToMembershipId = new Dictionary<string, int>();
        var paymentHistoryDetailEntitiesBDP = new List<PaymentHistoryDetailEntity>();
        string titleBDP = "Test Title";
        int totalRowCount = 0;

        _mockCreatePaymentDetail.Setup(d => d.CreatePaymentDetailSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(true);
        _mockGetLastPaymentID.Setup(d => d.GetLastPaymentIDSPAsync(It.IsAny<int>())).ReturnsAsync(1);
        _mockAllMembersInDBService.Setup(s => s.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Test User" } });

        // Act
        var result = await _service.OnActionBeginAsync(arg, selectedID, paymentToMembershipId, paymentHistoryDetailEntitiesBDP, titleBDP, totalRowCount);

        // Assert
        _mockCreatePaymentDetail.Verify(d => d.CreatePaymentDetailSPAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }
}