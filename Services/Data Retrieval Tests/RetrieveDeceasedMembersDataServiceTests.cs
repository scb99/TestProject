using DataAccess.Models;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;

namespace DataRetrieval;

public class RetrieveDeceasedMembersDataServiceTests
{
    [Fact]
    public void RetrieveDeceasedMembersData_ShouldReturnEmptyDictionary_WhenNoDeceasedMembers()
    {
        // Arrange
        var service = new RetrieveDeceasedMembersDataService();
        var volunteerLists = new VolunteerLists
        {
            deceasedMemberEntities = new List<DeceasedMemberEntity>()
        };

        // Act
        var result = service.RetrieveDeceasedMembersData(volunteerLists);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void RetrieveDeceasedMembersData_ShouldReturnDictionaryWithDeceasedMembers()
    {
        // Arrange
        var service = new RetrieveDeceasedMembersDataService();
        var volunteerLists = new VolunteerLists
        {
            deceasedMemberEntities = new List<DeceasedMemberEntity>
            {
                new() { ID = 1, LastName = "Doe", FirstName = "John" },
                new() { ID = 2, LastName = "Smith", FirstName = "Jane" }
            }
        };

        // Act
        var result = service.RetrieveDeceasedMembersData(volunteerLists);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey("Doe/John/1"));
        Assert.True(result.ContainsKey("Smith/Jane/2"));
    }

    [Fact]
    public void RetrieveDeceasedMembersData_ShouldNotAddDuplicateEntries()
    {
        // Arrange
        var service = new RetrieveDeceasedMembersDataService();
        var volunteerLists = new VolunteerLists
        {
            deceasedMemberEntities = new List<DeceasedMemberEntity>
            {
                new() { ID = 1, LastName = "Doe", FirstName = "John" },
                new() { ID = 1, LastName = "Doe", FirstName = "John" }
            }
        };

        // Act
        var result = service.RetrieveDeceasedMembersData(volunteerLists);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("Doe/John/1"));
    }
}