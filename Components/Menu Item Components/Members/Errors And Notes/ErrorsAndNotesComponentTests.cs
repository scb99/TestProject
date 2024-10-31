using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using Syncfusion.Blazor.Grids;

namespace MenuItemComponents;

public class ErrorsAndNotesComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingMemberDetailsService> _mockMemberDetailsService;
    private readonly Mock<ICrossCuttingMemberIDService> _mockMemberIDService;
    private readonly Mock<ICrossCuttingMemberNameService> _mockMemberNameService;
    private readonly Mock<ICrossCuttingNotesCenterService> _mockNotesCenterService;
    private readonly Mock<IRepositoryMemberDetail2> _mockMemberDetailRepository;
    private readonly Mock<IRepositoryGetCountOf<int>> _mockGetCountOfRepository;
    private readonly Mock<IRepositoryGetMembersWith<MemberEntity>> _mockGetMembersWithRepository;
    private readonly ErrorsAndNotesComponent _component;

    public ErrorsAndNotesComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockMemberDetailsService = new Mock<ICrossCuttingMemberDetailsService>();
        _mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        _mockNotesCenterService = new Mock<ICrossCuttingNotesCenterService>();
        _mockMemberDetailRepository = new Mock<IRepositoryMemberDetail2>();
        _mockGetCountOfRepository = new Mock<IRepositoryGetCountOf<int>>();
        _mockGetMembersWithRepository = new Mock<IRepositoryGetMembersWith<MemberEntity>>();

        _component = new ErrorsAndNotesComponent
        {
            Execute = _mockExecute.Object,
            Show = _mockShow.Object,
            AllMembersInDBService = _mockAllMembersInDBService.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            MemberDetailsService = _mockMemberDetailsService.Object,
            MemberIDService = _mockMemberIDService.Object,
            MemberNameService = _mockMemberNameService.Object,
            NotesCenterService = _mockNotesCenterService.Object,
            MemberDetailRepository = _mockMemberDetailRepository.Object,
            GetCountOfRepository = _mockGetCountOfRepository.Object,
            GetMembersWithRepository = _mockGetMembersWithRepository.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_InitializesCorrectly()
    {
        // Arrange
        _mockNotesCenterService.Setup(n => n.CurrentListOfNotes).Returns(new List<MemberEntity>());
        _mockNotesCenterService.Setup(n => n.StartUpNotes).Returns(new List<MemberEntity>());
        _mockGetCountOfRepository.Setup(r => r.GetCountOfMembersInGoodStandingAsync()).ReturnsAsync(10);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfPastDue0To30DaysAsync()).ReturnsAsync(5);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfPastDue31To60DaysAsync()).ReturnsAsync(3);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfPastDue61To365DaysAsync()).ReturnsAsync(2);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfMembershipsExpiringInNext60DaysAsync()).ReturnsAsync(1);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfMembersInGoodStandingTakingRosterAsync()).ReturnsAsync(4);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfMembersInGoodStandingWithoutEmailAddressesAsync()).ReturnsAsync(6);
        _mockGetCountOfRepository.Setup(r => r.GetCountOfExpiredMembershipsAsync()).ReturnsAsync(7);

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        _mockLoadingPanelService.Verify(l => l.ShowLoadingPanelAsync(), Times.Once);
        _mockNotesCenterService.Verify(n => n.FireOnNotesCenterChange(It.IsAny<List<MemberEntity>>()), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task OnRefreshButtonClickedAsync_RefreshesNotesCorrectly()
    {
        // Arrange
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithMultipleActiveRecordsAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithNoQuickBooksEntriesAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithTooManyQuickBooksEntriesAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithDuplicateQuickBooksPaymentsAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithDuplicateAccountsAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithBadHomePhoneNumbersAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithBadOtherPhoneNumbersAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithBadStateNamesAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithBadZipCodesAsync()).ReturnsAsync(new List<MemberEntity>());
        _mockGetMembersWithRepository.Setup(r => r.GetMembersWithBadBirthDatesAsync()).ReturnsAsync(new List<MemberEntity>());

        // Act
        await _component.OnRefreshButtonClickedAsync();

        // Assert
        _mockLoadingPanelService.Verify(l => l.ShowLoadingPanelAsync(), Times.Once);
        _mockLogger.Verify(l => l.LogResultAsync("Refresh button in ErrorsAndNotesComponent clicked"), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task OnSelectedRowChangedAsync_HandlesRowSelectionCorrectly()
    {
        // Arrange
        var memberEntity = new MemberEntity { ID = 1, Name = "Doe, John" };
        var args = new RowSelectEventArgs<MemberEntity> { Data = memberEntity };
        _mockAllMembersInDBService.Setup(a => a.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "John Doe" } });
        _mockMemberDetailRepository.Setup(r => r.GetMemberDetailsAsync(1)).ReturnsAsync(new List<MemberDetailEntity>());

        // Act
        await _component.OnSelectedRowChangedAsync(args);

        // Assert
        _mockLogger.Verify(l => l.LogResultAsync("Selected member (in ErrorsAndNotesComponent) with id= 1 (John Doe)"), Times.Once);
        _mockMemberDetailRepository.Verify(r => r.GetMemberDetailsAsync(1), Times.Once);
        _mockMemberIDService.VerifySet(m => m.MemberID = 1, Times.Once);
        _mockMemberNameService.VerifySet(m => m.MemberName = "John Doe", Times.Once);
        _mockMemberNameService.VerifySet(m => m.MemberFirstName = "John", Times.Once);
        _mockMemberNameService.VerifySet(m => m.MemberLastName = "Doe", Times.Once);
    }

    [Fact]
    public void AddListOfNotesToDisplayableList_AddsNotesCorrectly()
    {
        // Arrange
        var listOfNotes = new List<MemberEntity> { new() { ID = 1, Name = "Note 1" } };
        _mockNotesCenterService.Setup(n => n.CurrentListOfNotes).Returns(new List<MemberEntity>());
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        _component.AddListOfNotesToDisplayableList(listOfNotes);

        // Assert
        Assert.Contains(listOfNotes[0], _component.MemberEntitiesToDisplayBDP);
    }

    [Fact]
    public void AddMemberEntityListToListOfNotes_AddsMemberEntitiesCorrectly()
    {
        // Arrange
        var list = new List<MemberEntity> { new() { ID = 1, Name = "Member 1" } };
        _mockAllMembersInDBService.Setup(a => a.MemberNameDictionary).Returns(new Dictionary<int, string> { { 1, "Member 1" } });
        _mockNotesCenterService.Setup(n => n.CurrentListOfNotes).Returns(new List<MemberEntity>());
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        _component.AddMemberEntityListToListOfNotes("Test Message", list);

        // Assert
        Assert.Contains(list[0], _component.MemberEntitiesToDisplayBDP);
    }
}