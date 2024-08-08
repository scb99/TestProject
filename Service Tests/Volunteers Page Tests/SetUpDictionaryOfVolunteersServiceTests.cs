using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using Moq;
using static DBExplorerBlazor.Pages.VolunteersPage;

namespace MenuItemComponents;

public class SetUpDictionaryOfVolunteersServiceTests
{
    private readonly Mock<IVolunteerPageEntitySetupService> _mockEntitySetupService;
    private readonly SetUpDictionaryOfVolunteersService _service;

    public SetUpDictionaryOfVolunteersServiceTests()
    {
        _mockEntitySetupService = new Mock<IVolunteerPageEntitySetupService>();
        _service = new SetUpDictionaryOfVolunteersService(_mockEntitySetupService.Object);
    }

    [Fact]
    public void SetUpDictionaryOfVolunteers_CallsSetUpEntityForEachVolunteerType()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>();

        // Act
        var result = _service.SetUpDictionaryOfVolunteers(volunteerLists, deceasedMembers);

        // Assert
        _mockEntitySetupService.Verify(s => s.SetUpEntity(volunteerLists.annualMeetingEntities, VolunteerEnum.AnnualMeeting, It.IsAny<SortedDictionary<string, TypeOfVolunteer>>(), deceasedMembers), Times.Once);
        _mockEntitySetupService.Verify(s => s.SetUpEntity(volunteerLists.boardMemberEntities, VolunteerEnum.BoardMember, It.IsAny<SortedDictionary<string, TypeOfVolunteer>>(), deceasedMembers), Times.Once);
        _mockEntitySetupService.Verify(s => s.SetUpEntity(volunteerLists.partyVolunteerEntities, VolunteerEnum.PartyVolunteer, It.IsAny<SortedDictionary<string, TypeOfVolunteer>>(), deceasedMembers), Times.Once);
        _mockEntitySetupService.Verify(s => s.SetUpEntity(volunteerLists.tournamentVolunteerEntities, VolunteerEnum.TournamentVolunteer, It.IsAny<SortedDictionary<string, TypeOfVolunteer>>(), deceasedMembers), Times.Once);
    }

    [Fact]
    public void SetUpDictionaryOfVolunteers_ReturnsInitializedDictionary()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>();

        // Act
        var result = _service.SetUpDictionaryOfVolunteers(volunteerLists, deceasedMembers);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SortedDictionary<string, TypeOfVolunteer>>(result);
    }
}