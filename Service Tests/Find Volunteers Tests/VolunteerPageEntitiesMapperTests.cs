using DataAccess.Models;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using static DBExplorerBlazor.Pages.VolunteersPage;

namespace MenuItemComponents;

public class VolunteerPageEntitiesMapperTests
{
    private readonly VolunteerPageEntitiesMapper _mapper;

    public VolunteerPageEntitiesMapperTests()
    {
        _mapper = new VolunteerPageEntitiesMapper();
    }

    [Fact]
    public void MapVolunteerEntities_MapsEntitiesCorrectly()
    {
        // Arrange
        var volunteerEntitiesMapping = new Dictionary<VolunteerEnum, List<VolunteerEntity>>
        {
            { VolunteerEnum.AnnualMeeting, new List<VolunteerEntity> { new() { Name = "John Doe" } } },
            { VolunteerEnum.BoardMember, new List<VolunteerEntity> { new() { Name = "Jane Smith" } } },
            { VolunteerEnum.PartyVolunteer, new List<VolunteerEntity> { new() { Name = "Alice Johnson" } } },
            { VolunteerEnum.TournamentVolunteer, new List<VolunteerEntity> { new() { Name = "Bob Brown" } } }
        };

        var volunteerEntitiesBDP = new List<Volunteer>
        {
            new(),
            new(),
            new(),
            new()
        };

        // Act
        _mapper.MapVolunteerEntities(volunteerEntitiesMapping, volunteerEntitiesBDP);

        // Assert
        Assert.Equal("John Doe", volunteerEntitiesBDP[0].AnnualMeeting);
        Assert.Equal("Jane Smith", volunteerEntitiesBDP[0].BoardMember);
        Assert.Equal("Alice Johnson", volunteerEntitiesBDP[0].PartyVolunteer);
        Assert.Equal("Bob Brown", volunteerEntitiesBDP[0].TournamentVolunteer);
    }

    [Fact]
    public void MapVolunteerEntities_DoesNotThrowException_WhenEntitiesListIsEmpty()
    {
        // Arrange
        var volunteerEntitiesMapping = new Dictionary<VolunteerEnum, List<VolunteerEntity>>
        {
            { VolunteerEnum.AnnualMeeting, new List<VolunteerEntity>() }
        };

        var volunteerEntitiesBDP = new List<Volunteer>
        {
            new()
        };

        // Act & Assert
        var exception = Record.Exception(() => _mapper.MapVolunteerEntities(volunteerEntitiesMapping, volunteerEntitiesBDP));
        Assert.Null(exception);
    }
}