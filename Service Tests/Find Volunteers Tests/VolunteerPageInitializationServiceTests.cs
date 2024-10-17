using DataAccess.Models;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;

namespace MenuItemComponents;

public class VolunteerPageInitializationServiceTests
{
    private readonly VolunteerPageInitializationService _initializationService;

    public VolunteerPageInitializationServiceTests()
    {
        _initializationService = new VolunteerPageInitializationService();
    }

    [Fact]
    public void InitializationBeforeProcessingVolunteers_ShouldInitializeVolunteerEntitiesBDP()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>
        {
            { "John Doe", new TypeOfVolunteer { Name = "John Doe", AnnualMeetingBool = true } },
            { "Jane Smith", new TypeOfVolunteer { Name = "Jane Smith", BoardMemberBool = true } },
            { "Alice Johnson", new TypeOfVolunteer { Name = "Alice Johnson", PartyVolunteerBool = true } },
            { "Bob Brown", new TypeOfVolunteer { Name = "Bob Brown", TournamentVolunteerBool = true } }
        };
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>
        {
            new() { Name = "John Doe" },
            new() { Name = "Jane Smith" },
            new() { Name = "Alice Johnson" },
            new() { Name = "Bob Brown" }
        };

        // Act
        _initializationService.InitializationBeforeProcessingVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP);

        // Assert
        Assert.Equal(4, volunteersEntities2BDP.Count);
        Assert.Equal("John Doe", volunteersEntities2BDP[0].Name);
        Assert.Equal("Jane Smith", volunteersEntities2BDP[1].Name);
        Assert.Equal("Alice Johnson", volunteersEntities2BDP[2].Name);
        Assert.Equal("Bob Brown", volunteersEntities2BDP[3].Name);
    }

    [Fact]
    public void InitializationBeforeProcessingVolunteers_ShouldUpdateVolunteersEntities2BDP()
    {
        // Arrange
        var volunteerLists = new VolunteerLists
        {
            annualMeetingEntities = new List<VolunteerEntity> { new() { Name = "John Doe" } },
            boardMemberEntities = new List<VolunteerEntity> { new() { Name = "Jane Smith" } },
            partyVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Alice Johnson" } },
            tournamentVolunteerEntities = new List<VolunteerEntity> { new() { Name = "Bob Brown" } }
        };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>
        {
            { "John Doe", new TypeOfVolunteer { Name = "John Doe", AnnualMeetingBool = true } },
            { "Jane Smith", new TypeOfVolunteer { Name = "Jane Smith", BoardMemberBool = true } },
            { "Alice Johnson", new TypeOfVolunteer { Name = "Alice Johnson", PartyVolunteerBool = true } },
            { "Bob Brown", new TypeOfVolunteer { Name = "Bob Brown", TournamentVolunteerBool = true } }
        };
        var volunteerEntitiesBDP = new List<Volunteer>();
        var volunteersEntities2BDP = new List<TypeOfVolunteer>
        {
            new() { Name = "John Doe" },
            new() { Name = "Jane Smith" },
            new() { Name = "Alice Johnson" },
            new() { Name = "Bob Brown" }
        };

        // Act
        _initializationService.InitializationBeforeProcessingVolunteers(volunteerLists, volunteers, volunteerEntitiesBDP, volunteersEntities2BDP);

        // Assert
        Assert.True(volunteersEntities2BDP[0].AnnualMeetingBool);
        Assert.True(volunteersEntities2BDP[1].BoardMemberBool);
        Assert.True(volunteersEntities2BDP[2].PartyVolunteerBool);
        Assert.True(volunteersEntities2BDP[3].TournamentVolunteerBool);
        Assert.Equal("Yes", volunteersEntities2BDP[0].AnnualMeetingString);
        Assert.Equal("Yes", volunteersEntities2BDP[1].BoardMemberString);
        Assert.Equal("Yes", volunteersEntities2BDP[2].PartyVolunteerString);
        Assert.Equal("Yes", volunteersEntities2BDP[3].TournamentVolunteerString);
    }
}