using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor;
using Moq;

namespace DataRetrieval;

public class RetrieveVolunteersDataServiceTests
{
    [Fact]
    public async Task RetrieveVolunteersDataAsync_ShouldReturnVolunteerLists()
    {
        // Arrange
        var mockGetDeceasedMembers = new Mock<IGetDeceasedMembers>();
        var mockGetVolunteers = new Mock<IGetVolunteers>();
        mockGetDeceasedMembers.Setup(dm => dm.GetDeceasedMembersSPAsync()).ReturnsAsync(new List<DeceasedMemberEntity>());
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("annual_meeting")).ReturnsAsync(new List<VolunteerEntity>());
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("board_member")).ReturnsAsync(new List<VolunteerEntity>());
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("party_volunteer")).ReturnsAsync(new List<VolunteerEntity>());
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("tournament_volunteer")).ReturnsAsync(new List<VolunteerEntity>());

        var service = new RetrieveVolunteersDataService(mockGetDeceasedMembers.Object, mockGetVolunteers.Object);

        // Act
        var result = await service.RetrieveVolunteersDataAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.deceasedMemberEntities);
        Assert.Empty(result.annualMeetingEntities);
        Assert.Empty(result.boardMemberEntities);
        Assert.Empty(result.partyVolunteerEntities);
        Assert.Empty(result.tournamentVolunteerEntities);
    }

    [Fact]
    public async Task RetrieveVolunteersDataAsync_ShouldReturnPopulatedVolunteerLists()
    {
        // Arrange
        var mockGetDeceasedMembers = new Mock<IGetDeceasedMembers>();
        var mockGetVolunteers = new Mock<IGetVolunteers>();
        var deceasedMembers = new List<DeceasedMemberEntity> { new() { ID = 1, LastName = "Doe", FirstName = "John" } };
        var annualMeetingVolunteers = new List<VolunteerEntity> { new() { ID = "1", Name = "Annual Meeting Volunteer" } };
        var boardMemberVolunteers = new List<VolunteerEntity> { new() { ID = "2", Name = "Board Member Volunteer" } };
        var partyVolunteers = new List<VolunteerEntity> { new() { ID = "3", Name = "Party Volunteer" } };
        var tournamentVolunteers = new List<VolunteerEntity> { new() { ID = "4", Name = "Tournament Volunteer" } };

        mockGetDeceasedMembers.Setup(dm => dm.GetDeceasedMembersSPAsync()).ReturnsAsync(deceasedMembers);
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("annual_meeting")).ReturnsAsync(annualMeetingVolunteers);
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("board_member")).ReturnsAsync(boardMemberVolunteers);
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("party_volunteer")).ReturnsAsync(partyVolunteers);
        mockGetVolunteers.Setup(dm => dm.GetVolunteersSPAsync("tournament_volunteer")).ReturnsAsync(tournamentVolunteers);

        var service = new RetrieveVolunteersDataService(mockGetDeceasedMembers.Object, mockGetVolunteers.Object);

        // Act
        var result = await service.RetrieveVolunteersDataAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.deceasedMemberEntities);
        Assert.Single(result.annualMeetingEntities);
        Assert.Single(result.boardMemberEntities);
        Assert.Single(result.partyVolunteerEntities);
        Assert.Single(result.tournamentVolunteerEntities);
    }
}