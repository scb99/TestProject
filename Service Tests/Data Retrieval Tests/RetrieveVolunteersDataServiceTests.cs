using DataAccess.Models;
using DataAccessCommands.Interfaces;
using DBExplorerBlazor;
using Moq;

namespace DataRetrieval;

public class RetrieveVolunteersDataServiceTests
{
    private readonly Mock<IRepository<DeceasedMemberEntity>> mockDeceasedMemberRepository;
    private readonly Mock<IRepositoryVolunteers> mockGetVolunteers;
    private readonly RetrieveVolunteersDataService service;

    public RetrieveVolunteersDataServiceTests()
    {
        mockDeceasedMemberRepository = new Mock<IRepository<DeceasedMemberEntity>>();
        mockGetVolunteers = new Mock<IRepositoryVolunteers>();
        service = new RetrieveVolunteersDataService(mockDeceasedMemberRepository.Object, mockGetVolunteers.Object);
    }

    [Fact]
    public async Task RetrieveVolunteersDataAsync_ShouldReturnVolunteerLists()
    {
        // Arrange
        var deceasedMembers = new List<DeceasedMemberEntity>
        {
            new() { ID = 1, FirstName = "John", LastName = "Doe" }
        };
        var annualMeetingVolunteers = new List<VolunteerEntity>
        {
            new() { ID = "1", Name = "Jane Doe" }
        };
        var boardMemberVolunteers = new List<VolunteerEntity>
        {
            new() { ID = "2", Name = "Jim Beam" }
        };
        var partyVolunteers = new List<VolunteerEntity>
        {
            new() { ID = "3", Name = "Jack Daniels" }
        };
        var tournamentVolunteers = new List<VolunteerEntity>
        {
            new() { ID = "4", Name = "Johnny Walker" }
        };

        mockDeceasedMemberRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(deceasedMembers);
        mockGetVolunteers.Setup(vol => vol.GetVolunteersAsync("annual_meeting")).ReturnsAsync(annualMeetingVolunteers);
        mockGetVolunteers.Setup(vol => vol.GetVolunteersAsync("board_member")).ReturnsAsync(boardMemberVolunteers);
        mockGetVolunteers.Setup(vol => vol.GetVolunteersAsync("party_volunteer")).ReturnsAsync(partyVolunteers);
        mockGetVolunteers.Setup(vol => vol.GetVolunteersAsync("tournament_volunteer")).ReturnsAsync(tournamentVolunteers);

        // Act
        var result = await service.RetrieveVolunteersDataAsync();

        // Assert
        Assert.Equal(deceasedMembers, result.deceasedMemberEntities);
        Assert.Equal(annualMeetingVolunteers, result.annualMeetingEntities);
        Assert.Equal(boardMemberVolunteers, result.boardMemberEntities);
        Assert.Equal(partyVolunteers, result.partyVolunteerEntities);
        Assert.Equal(tournamentVolunteers, result.tournamentVolunteerEntities);
    }
}