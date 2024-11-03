using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersList;

public class MembersListFilteringServiceTests
{
    private readonly Mock<ICrossCuttingAllMembersInDBService> _allMembersInDBServiceMock;
    private readonly Mock<IAdministratorAccountsStrategy> _administratorAccountsStrategyMock;
    private readonly Mock<IAdminRoleFilteringStrategy> _adminRoleFilteringStrategyMock;
    private readonly Mock<IBoardMemberRoleFilterStrategy> _boardMemberRoleFilterStrategyMock;
    private readonly Mock<IDeceasedMembersFilterStrategy> _deceasedMembersFilterStrategyMock;
    private readonly Mock<IFirstNameFilterStrategy> _firstNameFilterStrategyMock;
    private readonly Mock<IIDFilteringStrategy> _idFilteringStrategyMock;
    private readonly Mock<ILastNameFilterStrategy> _lastNameFilterStrategyMock;
    private readonly Mock<ILifetimeMembersFilterStrategy> _lifetimeMembersFilterStrategyMock;
    private readonly Mock<IRemoveBadMemberDataRecordsFilterStrategy> _removeBadMemberDataRecordsFilterStrategyMock;
    private readonly Mock<ISuperUserRoleFilterStrategy> _superUserRoleFilterStrategyMock;
    private readonly Mock<ITestAccountFilterStrategy> _testAccountFilterStrategyMock;
    private readonly MembersListFilteringService _service;

    public MembersListFilteringServiceTests()
    {
        _allMembersInDBServiceMock = new Mock<ICrossCuttingAllMembersInDBService>();
        _administratorAccountsStrategyMock = new Mock<IAdministratorAccountsStrategy>();
        _adminRoleFilteringStrategyMock = new Mock<IAdminRoleFilteringStrategy>();
        _boardMemberRoleFilterStrategyMock = new Mock<IBoardMemberRoleFilterStrategy>();
        _deceasedMembersFilterStrategyMock = new Mock<IDeceasedMembersFilterStrategy>();
        _firstNameFilterStrategyMock = new Mock<IFirstNameFilterStrategy>();
        _idFilteringStrategyMock = new Mock<IIDFilteringStrategy>();
        _lastNameFilterStrategyMock = new Mock<ILastNameFilterStrategy>();
        _lifetimeMembersFilterStrategyMock = new Mock<ILifetimeMembersFilterStrategy>();
        _removeBadMemberDataRecordsFilterStrategyMock = new Mock<IRemoveBadMemberDataRecordsFilterStrategy>();
        _superUserRoleFilterStrategyMock = new Mock<ISuperUserRoleFilterStrategy>();
        _testAccountFilterStrategyMock = new Mock<ITestAccountFilterStrategy>();

        _service = new MembersListFilteringService(
            _allMembersInDBServiceMock.Object,
            _administratorAccountsStrategyMock.Object,
            _adminRoleFilteringStrategyMock.Object,
            _boardMemberRoleFilterStrategyMock.Object,
            _deceasedMembersFilterStrategyMock.Object,
            _firstNameFilterStrategyMock.Object,
            _idFilteringStrategyMock.Object,
            _lastNameFilterStrategyMock.Object,
            _lifetimeMembersFilterStrategyMock.Object,
            _removeBadMemberDataRecordsFilterStrategyMock.Object,
            _superUserRoleFilterStrategyMock.Object,
            _testAccountFilterStrategyMock.Object
        );
    }

    [Fact]
    public async Task FilterMembersAsync_EmptyFilterValue_ReturnsAllMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _allMembersInDBServiceMock.Setup(s => s.GetAllMembersInDBAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Last Name", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public void FilterMembersAsync_FilterByLastName_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _allMembersInDBServiceMock.Setup(s => s.AllMembersInDB).Returns(members);
        _lastNameFilterStrategyMock.Setup(s => s.Filter(members, "Smith")).Returns(members);

        // Act
        var result = _service.FilterMembersAsync("Filter by Last Name", "Smith").Result;

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public void FilterMembersAsync_FilterByFirstName_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _allMembersInDBServiceMock.Setup(s => s.AllMembersInDB).Returns(members);
        _firstNameFilterStrategyMock.Setup(s => s.Filter(members, "John")).Returns(members);

        // Act
        var result = _service.FilterMembersAsync("Filter by First Name", "John").Result;

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public void FilterMembersAsync_FilterByID_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _allMembersInDBServiceMock.Setup(s => s.AllMembersInDB).Returns(members);
        _idFilteringStrategyMock.Setup(s => s.Filter(members, "123")).Returns(members);

        // Act
        var result = _service.FilterMembersAsync("Filter by ID", "123").Result;

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByAdminRole_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _adminRoleFilteringStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Admin Role", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByAdministratorAccounts_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _administratorAccountsStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Administrator Accounts", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByBoardMemberRole_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _boardMemberRoleFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by BoardMember Role", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByDeceasedMembers_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _deceasedMembersFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Deceased Members", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByLifetimeMembers_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _lifetimeMembersFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Lifetime Members", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterBySuperUserRole_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _superUserRoleFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by SuperUser Role", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_FilterByTestAccounts_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _testAccountFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Filter by Test Accounts", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task FilterMembersAsync_RemoveBadMemberDataRecords_ReturnsFilteredMembers()
    {
        // Arrange
        var members = new List<MemberEntity> { new() };
        _removeBadMemberDataRecordsFilterStrategyMock.Setup(s => s.FilterAsync()).ReturnsAsync(members);

        // Act
        var result = await _service.FilterMembersAsync("Remove Bad Member Data Records", string.Empty);

        // Assert
        Assert.Equal(members, result);
    }
}