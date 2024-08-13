using DataAccess.Models;
using DBExplorerBlazor.Services;

namespace MemberPaymentHistory;

public class MemberPaymentHistoryTitleUpdaterTests
{
    private readonly MemberPaymentHistoryTitleUpdater _titleUpdater;

    public MemberPaymentHistoryTitleUpdaterTests()
    {
        _titleUpdater = new MemberPaymentHistoryTitleUpdater();
    }

    [Fact]
    public void GetTitle_ShouldReturnCorrectTitle_WhenPaymentHistoryIsEmpty()
    {
        // Arrange
        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity>();
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitle(paymentHistoryDetails, memberName);

        // Assert
        Assert.Equal("0 payments for John Doe", result);
    }

    [Fact]
    public void GetTitle_ShouldReturnCorrectTitle_WhenMembershipHasExpired()
    {
        // Arrange
        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity>
        {
            new() { EndDate = DateTime.Today.AddDays(-1) }
        };
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitle(paymentHistoryDetails, memberName);

        // Assert
        Assert.Equal("1 payment for John Doe, membership has expired", result);
    }

    [Fact]
    public void GetTitle_ShouldReturnCorrectTitle_WhenMemberIsInGoodStanding()
    {
        // Arrange
        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity>
        {
            new() { EndDate = DateTime.Today.AddDays(1) }
        };
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitle(paymentHistoryDetails, memberName);

        // Assert
        Assert.Equal("1 payment for John Doe, member in good standing", result);
    }

    [Fact]
    public void GetTitle_ShouldReturnCorrectTitle_WhenMemberReceivesRoster()
    {
        // Arrange
        var paymentHistoryDetails = new List<PaymentHistoryDetailEntity>
        {
            new() { EndDate = DateTime.Today.AddDays(1), MembershipID = 2 }
        };
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitle(paymentHistoryDetails, memberName);

        // Assert
        Assert.Equal("1 payment for John Doe, member in good standing and receives roster", result);
    }

    [Fact]
    public void GetTitleTypeOfAccount_ShouldReturnCorrectTitle_WhenMemberIsAdministrator()
    {
        // Arrange
        var memberDetailEntities = new List<MemberDetailEntity>
        {
            new() { Property = "administrator", Value = "Yes" }
        };
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitleTypeOfAccount(memberDetailEntities, memberName);

        // Assert
        Assert.Equal("John Doe is an administrator", result);
    }

    [Fact]
    public void GetTitleTypeOfAccount_ShouldReturnCorrectTitle_WhenMemberHasRegularAccount()
    {
        // Arrange
        var memberDetailEntities = new List<MemberDetailEntity>();
        var memberName = "John Doe";

        // Act
        var result = _titleUpdater.GetTitleTypeOfAccount(memberDetailEntities, memberName);

        // Assert
        Assert.Equal("John Doe has a regular account", result);
    }
}