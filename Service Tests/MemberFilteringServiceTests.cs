using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberFilteringServiceTests
{
    private readonly Mock<IAllMembersInDBService> _mockAllMembersInDBService = new();
    private readonly Mock<IAdminRoleFilteringStrategy> _mockAdminRoleFilteringStrategy = new ();
    private readonly Mock<IAdministratorAccountsStrategy> _mockAdministratorAccountsStrategy = new();
    private readonly Mock<IBoardMemberRoleFilterStrategy> _mockBoardMemberRoleFilterStrategy = new();
    private readonly Mock<IDeceasedMembersFilterStrategy> _mockDeceasedMembersFilterStrategy = new();
    private readonly Mock<IFirstNameFilterStrategy> _mockFirstNameFilterStrategy = new ();
    private readonly Mock<IIDFilteringStrategy> _mockIDFilteringStrategy = new();
    private readonly Mock<ILastNameFilterStrategy> _mockLastNameFilterStrategy = new();
    private readonly Mock<ILifetimeMembersFilterStrategy> _mockLifetimeMembersFilterStrategy = new();
    private readonly Mock<ISuperUserRoleFilterStrategy> _mockSuperUserRoleFilterStrategy = new();
    private readonly Mock<ITestAccountFilterStrategy> _mockTestAccountFilterStrategy = new ();
    
    private MemberFilteringService CreateService()
    {
        return new MemberFilteringService(
            _mockAdminRoleFilteringStrategy.Object,
            _mockAdministratorAccountsStrategy.Object,
            _mockAllMembersInDBService.Object,
            _mockBoardMemberRoleFilterStrategy.Object,
            _mockDeceasedMembersFilterStrategy.Object,
            _mockLifetimeMembersFilterStrategy.Object,
            _mockSuperUserRoleFilterStrategy.Object,
            _mockTestAccountFilterStrategy.Object,
            _mockFirstNameFilterStrategy.Object,
            _mockIDFilteringStrategy.Object,
            _mockLastNameFilterStrategy.Object
        );
    }

    [Fact]
    public async Task FilterMembersAsync_EmptyFilterValue_ReturnsAllMembers()
    {
        // Arrange
        var service = CreateService();
        var expectedMembers = new List<MemberEntity> { new(), new() };
        _mockAllMembersInDBService.Setup(s => s.GetAllMembersInDBAsync()).ReturnsAsync(expectedMembers);
        string filterCriteria = "Filter by Last Name";
        string filterValue = string.Empty;

        // Act
        var result = await service.FilterMembersAsync(filterCriteria, filterValue);

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
    }

    [Fact]
    public async Task FilterMembersAsync_WithFilterValue_CallsCorrectStrategy()
    {
        // Arrange
        var service = CreateService();
        var expectedMembers = new List<MemberEntity> { new() { LastName = "Doe" } };
        _mockLastNameFilterStrategy.Setup(s => s.Filter(It.IsAny<List<MemberEntity>>(), "Doe")).Returns(expectedMembers);
        string filterCriteria = "Filter by Last Name";
        string filterValue = "Doe";

        // Act
        var result = await service.FilterMembersAsync(filterCriteria, filterValue);

        // Assert
        _mockLastNameFilterStrategy.Verify(s => s.Filter(It.IsAny<List<MemberEntity>>(), "Doe"), Times.Once);
    }

    [Fact]
    public async Task FilterMembersAsync_UnknownFilterCriteria_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();
        string filterCriteria = "Unknown Criteria";
        string filterValue = "SomeValue";

        // Act
        var result = await service.FilterMembersAsync(filterCriteria, filterValue);

        // Assert
        Assert.Empty(result);
    }
}