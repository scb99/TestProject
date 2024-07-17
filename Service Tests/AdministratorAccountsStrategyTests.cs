using DataAccess;
using DataAccess.Models;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class AdministratorAccountsStrategyTests
{
    private readonly Mock<IDataManager> _dataManagerMock = new();
    private readonly AdministratorAccountsStrategy _administratorAccountsStrategy;

    public AdministratorAccountsStrategyTests() 
        => _administratorAccountsStrategy = new AdministratorAccountsStrategy(_dataManagerMock.Object);

    [Fact]
    public async Task FilterAsync_ReturnsAdministrators()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };
        _dataManagerMock.Setup(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("administrator", "Yes")).ReturnsAsync(expectedMembers);

        // Act
        var result = await _administratorAccountsStrategy.FilterAsync();

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        _dataManagerMock.Verify(dm => dm.GetMembersByMetaKeyAndMetaValueSPAsync("administrator", "Yes"), Times.Once);
    }
}