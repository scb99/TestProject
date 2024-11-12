using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using Syncfusion.Blazor.Grids;

namespace CrossCuttingConcerns;

public class MemberDetailsBaseServiceTests
{
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingAlertService> _mockAlertService;
    private readonly Mock<IRepositoryMemberDetail5> _mockMemberDetailRepository5;
    private readonly Mock<IRepository<EmailAddressEntity>> _mockEmailAddressRepository;
    private readonly Mock<IRepository<MemberDetailEntity>> _mockMemberDetailRepository;
    private readonly Mock<IRepositoryMemberDetail3> _mockMemberDetailRepository3;
    private readonly MemberDetailsBaseService _service;

    public MemberDetailsBaseServiceTests()
    {
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mockAlertService = new Mock<ICrossCuttingAlertService>();
        _mockMemberDetailRepository5 = new Mock<IRepositoryMemberDetail5>();
        _mockEmailAddressRepository = new Mock<IRepository<EmailAddressEntity>>();
        _mockMemberDetailRepository = new Mock<IRepository<MemberDetailEntity>>();
        _mockMemberDetailRepository3 = new Mock<IRepositoryMemberDetail3>();

        _service = new MemberDetailsBaseService(
            _mockAllMembersInDBService.Object,
            _mockLoggerService.Object,
            _mockAlertService.Object,
            _mockMemberDetailRepository5.Object,
            _mockEmailAddressRepository.Object,
            _mockMemberDetailRepository.Object,
            _mockMemberDetailRepository3.Object
        );
    }

    [Fact]
    public async Task OnActionBeginAsync_BeginEdit_ClonesData()
    {
        // Arrange
        var memberDetail = new MemberDetailEntity { ID = 1, UserID = 1, Property = "Test", Value = "Value" };
        var args = new ActionEventArgs<MemberDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.BeginEdit, Data = memberDetail };
        MemberDetailEntity? clonedMemberDetail = null;

        // Act
        var result = await _service.OnActionBeginAsync(args, clonedMemberDetail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(memberDetail.ID, result.ID);
        Assert.Equal(memberDetail.UserID, result.UserID);
        Assert.Equal(memberDetail.Property, result.Property);
        Assert.Equal(memberDetail.Value, result.Value);
    }

    [Fact]
    public async Task OnActionBeginAsync_Save_CreatesOrUpdatesRecord()
    {
        // Arrange
        var memberDetail = new MemberDetailEntity { ID = 0, UserID = 1, Property = "Test", Value = "Value" };
        var args = new ActionEventArgs<MemberDetailEntity> { RequestType = Syncfusion.Blazor.Grids.Action.Save, Data = memberDetail };
        MemberDetailEntity? clonedMemberDetail = null;

        _mockMemberDetailRepository.Setup(repo => repo.AddAsync(It.IsAny<MemberDetailEntity>())).ReturnsAsync(true);
        _mockMemberDetailRepository5.Setup(repo => repo.GetMemberDetailByUserIDAndMetaKeyReturningUmetaIDAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
        _mockLoggerService.Setup(service => service.LogResultAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mockAllMembersInDBService.Setup(_mockLoggerService => _mockLoggerService.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Test" } }); 
       
        // Act
        var result = await _service.OnActionBeginAsync(args, clonedMemberDetail);

        // Assert
        Assert.Null(result);
        _mockMemberDetailRepository.Verify(repo => repo.AddAsync(It.IsAny<MemberDetailEntity>()), Times.Once);
        _mockMemberDetailRepository5.Verify(repo => repo.GetMemberDetailByUserIDAndMetaKeyReturningUmetaIDAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }
}