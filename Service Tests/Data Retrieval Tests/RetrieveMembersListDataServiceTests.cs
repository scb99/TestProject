using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace DataRetrieval;

public class RetrieveMembersListDataServiceTests
{
    [Fact]
    public async Task LoadMembersAsync_CallsShowLoadingPanelAsync()
    {
        // Arrange
        var mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        var mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        var service = new RetrieveMembersListDataService(mockAllMembersInDBService.Object, mockLoadingPanelService.Object);

        // Act
        await service.RetrieveMembersAsync();

        // Assert
        mockLoadingPanelService.Verify(lps => lps.ShowLoadingPanelAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadMembersAsync_ReturnsAllMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity> { new() { /* Initialize properties as needed */ } };
        var mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        mockAllMembersInDBService.Setup(s => s.GetAllMembersInDBAsync()).ReturnsAsync(expectedMembers);
        var mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        var service = new RetrieveMembersListDataService(mockAllMembersInDBService.Object, mockLoadingPanelService.Object);

        // Act
        var result = await service.RetrieveMembersAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
        mockAllMembersInDBService.Verify(s => s.GetAllMembersInDBAsync(), Times.Once);
    }
}