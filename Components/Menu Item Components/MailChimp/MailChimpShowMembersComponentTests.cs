using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Moq;

namespace MenuItemComponents;

public class MailChimpShowMembersComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _mockLoggerService;
    private readonly Mock<ICrossCuttingMailChimpService> _mailChimpServiceMock;
    private readonly Mock<IMailChimpManager> _mailChimpManagerMock;
    private readonly MailChimpShowMembersComponent _component;

    public MailChimpShowMembersComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockLoggerService = new Mock<ICrossCuttingLoggerService>();
        _mailChimpServiceMock = new Mock<ICrossCuttingMailChimpService>();
        _mailChimpManagerMock = new Mock<IMailChimpManager>();

        _mailChimpServiceMock.Setup(m => m.MailChimpManager).Returns(_mailChimpManagerMock.Object);

        _component = new MailChimpShowMembersComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("LoggerService", _mockLoggerService.Object);
        _component.SetPrivatePropertyValue("MailChimpService", _mailChimpServiceMock.Object);
    }

    [Fact]
    public async Task OnInitializedAsync_SetsLoadingBDPToTrueAndCallsGetMailChimpMembersAsync()
    {
        // Arrange
        _mockExecute.Setup(x => x.ConditionalCode()).Returns(false);

        // Act
        await typeof(MailChimpShowMembersComponent).InvokeAsync("OnInitializedAsync", _component);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
        _mockExecute.Verify(x => x.ConditionalCode(), Times.Once);
    }

    [Fact]
    public async Task GetMailChimpMembersAsync_CallsGetAllMailChimpMembersAsyncAndLogsResults()
    {
        // Arrange
        _mockExecute.Setup(x => x.ConditionalCode()).Returns(false);

        // Act
        await typeof(MailChimpShowMembersComponent).InvokeAsync("GetMailChimpMembersAsync", _component);

        // Assert
        _mockLoggerService.Verify(x => x.LogResultAsync(It.IsAny<string>()), Times.Once);
        Assert.False(_component.GetPrivatePropertyValue<bool>("LoadingBDP"));
    }

    [Fact]
    public async Task GetAllMailChimpMembersAsync_CallsBuildMailChimpMembersDictionaryFromMailChimpWebSiteAsync()
    {
        // Act
        await _component.GetAllMailChimpMembersAsync();

        // Assert
        _mockLoggerService.Verify(x => x.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task BuildMailChimpMembersDictionaryFromMailChimpWebSiteAsync_PopulatesMailChimpMembers()
    {
        // Arrange
        var mockMembers = new List<Member>
        {
            new() 
            {
                MergeFields = new Dictionary<string, object> { { "LNAME", "Doe" }, { "FNAME", "John" } },
                Status = Status.Subscribed,
                EmailAddress = "john.doe@example.com",
                LastChanged = DateTime.Now.ToString()
            }
        };
        _mailChimpServiceMock.Setup(x => x.MailChimpManager.Members.GetAllAsync(It.IsAny<string>(), It.IsAny<MemberRequest>()))
            .ReturnsAsync(mockMembers);

        // Act
        await _component.BuildMailChimpMembersDictionaryFromMailChimpWebSiteAsync();

        // Assert
        Assert.Single(_component.MailChimpMembers);
        Assert.Equal("Doe, John", _component.MailChimpMembers.Keys.First());
    }
}