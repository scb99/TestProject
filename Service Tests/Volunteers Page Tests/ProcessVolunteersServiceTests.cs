using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using Moq;
using static DBExplorerBlazor.Pages.VolunteersPage;

namespace MenuItemComponents;

public class ProcessVolunteersServiceTests
{
    private readonly Mock<IVolunteerPageInitializationService> _mockInitializationService;
    private readonly Mock<IVolunteerPageEntitiesMapper> _mockEntitiesMapper;
    private readonly ProcessVolunteersService _service;

    public ProcessVolunteersServiceTests()
    {
        _mockInitializationService = new Mock<IVolunteerPageInitializationService>();
        _mockEntitiesMapper = new Mock<IVolunteerPageEntitiesMapper>();
        _service = new ProcessVolunteersService(_mockInitializationService.Object, _mockEntitiesMapper.Object);
    }

    [Fact]
    public void ProcessVolunteers_CallsInitializationAndMapping()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>();
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();

        // Act
        _service.ProcessVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP);

        // Assert
        _mockInitializationService.Verify(s => s.InitializationBeforeProcessingVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP), Times.Once);
        _mockEntitiesMapper.Verify(m => m.MapVolunteerEntities(It.IsAny<Dictionary<VolunteerEnum, List<VolunteerEntity>>>(), volunteerEntitiesBDP), Times.Once);
    }

    [Fact]
    public void ProcessVolunteers_MapsCorrectly()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>();
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>();

        var expectedMapping = new Dictionary<VolunteerEnum, List<VolunteerEntity>>
            {
                { VolunteerEnum.AnnualMeeting, volunteerLists.annualMeetingEntities },
                { VolunteerEnum.BoardMember, volunteerLists.boardMemberEntities },
                { VolunteerEnum.PartyVolunteer, volunteerLists.partyVolunteerEntities },
                { VolunteerEnum.TournamentVolunteer, volunteerLists.tournamentVolunteerEntities }
            };

        // Act
        _service.ProcessVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP);

        // Assert
        _mockEntitiesMapper.Verify(m => m.MapVolunteerEntities(It.Is<Dictionary<VolunteerEnum, List<VolunteerEntity>>>(d =>
            d[VolunteerEnum.AnnualMeeting] == expectedMapping[VolunteerEnum.AnnualMeeting] &&
            d[VolunteerEnum.BoardMember] == expectedMapping[VolunteerEnum.BoardMember] &&
            d[VolunteerEnum.PartyVolunteer] == expectedMapping[VolunteerEnum.PartyVolunteer] &&
            d[VolunteerEnum.TournamentVolunteer] == expectedMapping[VolunteerEnum.TournamentVolunteer]
        ), volunteerEntitiesBDP), Times.Once);
    }
}