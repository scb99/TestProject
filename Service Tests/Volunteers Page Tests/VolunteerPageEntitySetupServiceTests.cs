using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using DBExplorerBlazor.Services;
using Moq;
using static DBExplorerBlazor.Pages.VolunteersPage;

namespace MenuItemComponents;

public class VolunteerPageEntitySetupServiceTests
{
    private readonly Mock<IVolunteerPageKeyGeneratorService> _mockKeyGenerator;
    private readonly Mock<IVolunteerPageEntityFactory> _mockEntityFactory;
    private readonly VolunteerPageEntitySetupService _service;

    public VolunteerPageEntitySetupServiceTests()
    {
        _mockKeyGenerator = new Mock<IVolunteerPageKeyGeneratorService>();
        _mockEntityFactory = new Mock<IVolunteerPageEntityFactory>();
        _service = new VolunteerPageEntitySetupService(_mockKeyGenerator.Object, _mockEntityFactory.Object);
    }

    [Fact]
    public void SetUpEntity_CreatesAndUpdatesEntitiesCorrectly()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity { Name = "John Doe" };
        var volunteerEntities = new List<VolunteerEntity> { volunteerEntity };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>();
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>();
        var volunteerType = new TypeOfVolunteer();

        _mockKeyGenerator.Setup(k => k.GenerateKey(volunteerEntity)).Returns("key1");
        _mockEntityFactory.Setup(f => f.CreateVolunteerEntity(volunteerEntity)).Returns(volunteerType);

        // Act
        _service.SetUpEntity(volunteerEntities, VolunteerEnum.AnnualMeeting, volunteers, deceasedMembers);

        // Assert
        Assert.Single(volunteers);
        Assert.True(volunteers.ContainsKey("key1"));
        Assert.Equal(volunteerType, volunteers["key1"]);
        Assert.True(volunteers["key1"].AnnualMeetingBool);
    }

    [Fact]
    public void SetUpEntity_DoesNotCreateEntityIfKeyExistsInDeceasedMembers()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity { Name = "John Doe" };
        var volunteerEntities = new List<VolunteerEntity> { volunteerEntity };
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>();
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>
        {
            { "key1", new DeceasedMemberEntity() }
        };

        _mockKeyGenerator.Setup(k => k.GenerateKey(volunteerEntity)).Returns("key1");

        // Act
        _service.SetUpEntity(volunteerEntities, VolunteerEnum.AnnualMeeting, volunteers, deceasedMembers);

        // Assert
        Assert.Empty(volunteers);
    }

    [Fact]
    public void SetUpEntity_UpdatesExistingVolunteerProperties()
    {
        // Arrange
        var volunteerEntity = new VolunteerEntity { Name = "John Doe" };
        var volunteerEntities = new List<VolunteerEntity> { volunteerEntity };
        var volunteerType = new TypeOfVolunteer();
        var volunteers = new SortedDictionary<string, TypeOfVolunteer>
        {
            { "key1", volunteerType }
        };
        var deceasedMembers = new SortedDictionary<string, DeceasedMemberEntity>();

        _mockKeyGenerator.Setup(k => k.GenerateKey(volunteerEntity)).Returns("key1");

        // Act
        _service.SetUpEntity(volunteerEntities, VolunteerEnum.BoardMember, volunteers, deceasedMembers);

        // Assert
        Assert.Single(volunteers);
        Assert.True(volunteers["key1"].BoardMemberBool);
    }
}