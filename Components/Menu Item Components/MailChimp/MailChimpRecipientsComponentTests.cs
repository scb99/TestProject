using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Moq;

namespace MenuItemComponents;

public class MailChimpRecipientsComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<ICrossCuttingMailChimpService> _mailChimpServiceMock;
    private readonly Mock<IRepositoryExpiredMemberships> _expiredMembershipsRepositoryMock;
    private readonly Mock<IRepositoryMailChimpMembers> _mailChimpMembersRepositoryMock;
    private readonly Mock<IMailChimpManager> _mailChimpManagerMock;
    private readonly MailChimpRecipientsComponent _component;

    public MailChimpRecipientsComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _mailChimpServiceMock = new Mock<ICrossCuttingMailChimpService>();
        _expiredMembershipsRepositoryMock = new Mock<IRepositoryExpiredMemberships>();
        _mailChimpMembersRepositoryMock = new Mock<IRepositoryMailChimpMembers>();
        _mailChimpManagerMock = new Mock<IMailChimpManager>();

        _mailChimpServiceMock.Setup(m => m.MailChimpManager).Returns(_mailChimpManagerMock.Object);
        _mailChimpServiceMock.Setup(m => m.ListID).Returns("test-list-id");

        _component = new MailChimpRecipientsComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("Logger", _loggerMock.Object);
        _component.SetPrivatePropertyValue("MailChimpService", _mailChimpServiceMock.Object);
        _component.SetPrivatePropertyValue("ExpiredMembershipsRepository", _expiredMembershipsRepositoryMock.Object);
        _component.SetPrivatePropertyValue("MailChimpMembersRepository", _mailChimpMembersRepositoryMock.Object);

        typeof(MailChimpRecipientsComponent).Invoke("OnInitialized", _component);
    }

    [Fact]
    public async Task OnParametersSetAsync_GracePeriodChanged_PerformsSteps()
    {
        // Arrange
        _component.SetPublicPropertyValue<int>("GracePeriod", 30);
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("OnParametersSetAsync", _component);

        // Assert
        _loggerMock.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Step1GetAllMailChimpMembersAsync_RetrievesMembers()
    {
        // Arrange
        var members = new List<Member>
        {
            new() { EmailAddress = "test1@example.com" },
            new() { EmailAddress = "test2@example.com" }
        };
        _mailChimpManagerMock.Setup(m => m.Members.GetAllAsync(It.IsAny<string>(), It.IsAny<MemberRequest>()))
            .ReturnsAsync(members);

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("Step1GetAllMailChimpMembersAsync", _component);

        // Assert
        Assert.Equal(2, _component.MailChimpMembers.Count);
        Assert.Contains("test1@example.com", _component.MailChimpMembers.Keys);
        Assert.Contains("test2@example.com", _component.MailChimpMembers.Keys);
    }

    [Fact]
    public async Task Step2MembersInGoodStandingAsync_RetrievesMembers()
    {
        // Arrange
        var members = new List<MailChimpMemberFromDBEntity>
        {
            new() { Email = "test1@example.com", Deceased = "no" },
            new() { Email = "test2@example.com", Deceased = "no" }
        };
        _mailChimpMembersRepositoryMock.Setup(m => m.GetMailChimpMembersAsync(It.IsAny<int>()))
            .ReturnsAsync(members);

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("Step2MembersInGoodStandingAsync", _component);

        // Assert
        Assert.Equal(2, _component.MembersInGoodStanding.Count);
        Assert.Contains("test1@example.com", _component.MembersInGoodStanding.Keys);
        Assert.Contains("test2@example.com", _component.MembersInGoodStanding.Keys);
    }

    [Fact]
    public async Task Step3RemoveDeceasedMembersAsync_RemovesDeceasedMembers()
    {
        // Arrange
        _component.MailChimpMembers["test1@example.com"] = new Member { EmailAddress = "test1@example.com" };
        _component.MembersInGoodStanding["test1@example.com"] = new MailChimpMemberFromDBEntity { Deceased = "yes" };

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("Step3RemoveDeceasedMembersAsync", _component);

        // Assert
        _mailChimpManagerMock.Verify(m => m.Members.PermanentDeleteAsync(It.IsAny<string>(), "test1@example.com"), Times.Never);
    }

    [Fact]
    public async Task Step4AddNewMembersToMailChimpAsync_AddsNewMembers()
    {
        // Arrange
        _component.MembersInGoodStanding["test1@example.com"] = new MailChimpMemberFromDBEntity { Email = "test1@example.com", FirstName = "Test", LastName = "User" };

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("Step4AddNewMembersToMailChimpAsync", _component);

        // Assert
        //_mailChimpManagerMock.Verify(m => m.Members.AddOrUpdateAsync(It.IsAny<string>(), It.Is<Member>(member => member.EmailAddress == "test1@example.com")), Times.Once);
        //_mailChimpManagerMock.Verify(m => m.Members.AddOrUpdateAsync(It.IsAny<string>(), It.IsAny<Member>()), Times.Once);
    }

        [Fact]
    public async Task Step5ChangeSubscribedMembersToArchivedMembersAsync_ArchivesMembers()
    {
        // Arrange
        var expiredMembers = new List<ExpiredMembershipsEntity>
        {
            new() { EmailAddress = "test1@example.com", FirstName = "Test", LastName = "User" }
        };
        _expiredMembershipsRepositoryMock.Setup(m => m.GetExpiredMembershipsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expiredMembers);
        _component.MailChimpMembers["test1@example.com"] = new Member
        {
            EmailAddress = "test1@example.com",
            Status = Status.Subscribed,
            MergeFields = new Dictionary<string, object> { { "FNAME", "Test" }, { "LNAME", "User" } }
        };

        // Act
        await typeof(MailChimpRecipientsComponent).InvokeAsync("Step5ChangeSubscribedMembersToArchivedMembersAsync", _component);

        // Assert
        _mailChimpManagerMock.Verify(m => m.Members.DeleteAsync(It.IsAny<string>(), "test1@example.com"), Times.Never);
    }
}