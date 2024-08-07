using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using Moq;

namespace MenuItemComponents;

public class StateManagementVolunteersPageServiceTests
{
    private readonly Mock<IRetrieveDeceasedMembersDataService> _mockRetrieveDeceasedMembersDataService;
    private readonly Mock<ISetUpDictionaryOfVolunteers> _mockSetUpDictionaryOfVolunteersService;
    private readonly Mock<IProcessVolunteersService> _mockProcessVolunteersService;
    private readonly StateManagementVolunteersPageService _stateManagementService;

    public StateManagementVolunteersPageServiceTests()
    {
        _mockRetrieveDeceasedMembersDataService = new Mock<IRetrieveDeceasedMembersDataService>();
        _mockSetUpDictionaryOfVolunteersService = new Mock<ISetUpDictionaryOfVolunteers>();
        _mockProcessVolunteersService = new Mock<IProcessVolunteersService>();

        _stateManagementService = new StateManagementVolunteersPageService(
            _mockRetrieveDeceasedMembersDataService.Object,
            _mockSetUpDictionaryOfVolunteersService.Object,
            _mockProcessVolunteersService.Object
        );
    }

    [Fact]
    public void ManageState_ShouldReturnCorrectData()
    {
        // Arrange
        var volunteerLists = new VolunteerLists();
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>();
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();
        var volunteerEntitiesBDP = new List<Volunteer>();

        _mockRetrieveDeceasedMembersDataService
            .Setup(service => service.RetrieveDeceasedMembersData(volunteerLists))
            .Returns(deceasedMembers);

        _mockSetUpDictionaryOfVolunteersService
            .Setup(service => service.SetUpDictionaryOfVolunteers(volunteerLists, deceasedMembers))
            .Returns(volunteers);

        _mockProcessVolunteersService
            .Setup(service => service.ProcessVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP));

        // Act
        var result = _stateManagementService.ManageState(volunteerLists);

        // Assert
        Assert.Equal(volunteerLists, result.Item1);
        Assert.Equal(volunteerEntitiesBDP, result.Item2);
        Assert.Equal(volunteersEntities2BDP, result.Item3);

        _mockRetrieveDeceasedMembersDataService.Verify(service => service.RetrieveDeceasedMembersData(volunteerLists), Times.Once);
        _mockSetUpDictionaryOfVolunteersService.Verify(service => service.SetUpDictionaryOfVolunteers(volunteerLists, deceasedMembers), Times.Once);
        _mockProcessVolunteersService.Verify(service => service.ProcessVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP), Times.Once);
    }
}