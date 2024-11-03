using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace MenuItemPages;

public class MembersPageTests
{
    [Fact]
    public void IsAuthorized_CallsAuthorizationService()
    {
        // Arrange
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        mockAuthorizationService.Setup(service => service.IsAuthorized()).Returns(true);

        var page = new MembersPage
        {
            AuthorizationService = mockAuthorizationService.Object,
            LoggedInMemberService = mockLoggedInMemberService.Object,
            MemberIDService = mockMemberIDService.Object
        };

        // Act
        var result = page.IsAuthorized();

        // Assert
        Assert.True(result);
        mockAuthorizationService.Verify(service => service.IsAuthorized(), Times.Once);
    }

    [Fact]
    public void OnInitialized_SubscribesToMemberIDOnChange()
    {
        // Arrange
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        var page = new MembersPage
        {
            AuthorizationService = mockAuthorizationService.Object,
            LoggedInMemberService = mockLoggedInMemberService.Object,
            MemberIDService = mockMemberIDService.Object
        };

        // Act
        page.OnInitialized2();

        // Assert
        mockMemberIDService.VerifyAdd(service => service.MemberIDOnChange += It.IsAny<Action>(), Times.Once);
    }

    [Fact]
    public void Dispose_UnsubscribesFromMemberIDOnChange()
    {
        // Arrange
        var mockAuthorizationService = new Mock<IAuthorizationService>();
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

        var page = new MembersPage
        {
            AuthorizationService = mockAuthorizationService.Object,
            LoggedInMemberService = mockLoggedInMemberService.Object,
            MemberIDService = mockMemberIDService.Object
        };

        // Act
        page.Dispose();

        // Assert
        mockMemberIDService.VerifyRemove(service => service.MemberIDOnChange -= It.IsAny<Action>(), Times.Once);
    }

    //[Fact]
    //public void MemberIDOnChange_InvokesStateHasChanged()
    //{
    //    // Arrange
    //    var mockAuthorizationService = new Mock<IAuthorizationService>();
    //    var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
    //    var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();

    //    var page = new MembersPage
    //    {
    //        AuthorizationService = mockAuthorizationService.Object,
    //        LoggedInMemberService = mockLoggedInMemberService.Object,
    //        MemberIDService = mockMemberIDService.Object
    //    };

    //    var stateHasChangedInvoked = false;
    //    page.StateHasChanged = () => stateHasChangedInvoked = true;

    //    // Act
    //    page.MemberIDOnChange();

    //    // Assert
    //    Assert.True(stateHasChangedInvoked);
    //}
}