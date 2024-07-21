namespace Service;

using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class MembersLoadServiceTests
{
    [Fact]
    public async Task LoadMembersAsync_CallsShowLoadingPanelAsync()
    {
        // Arrange
        var mockAllMembersInDBService = new Mock<IAllMembersInDBService>();
        var mockLoadingPanelService = new Mock<ILoadingPanelService>();
        var service = new MembersLoadService(mockAllMembersInDBService.Object, mockLoadingPanelService.Object);

        // Act
        await service.LoadMembersAsync();

        // Assert
        mockLoadingPanelService.Verify(lps => lps.ShowLoadingPanelAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadMembersAsync_ReturnsAllMembers()
    {
        // Arrange
        var expectedMembers = new List<MemberEntity> { new() { /* Initialize properties as needed */ } };
        var mockAllMembersInDBService = new Mock<IAllMembersInDBService>();
        mockAllMembersInDBService.Setup(s => s.GetAllMembersInDBAsync()).ReturnsAsync(expectedMembers);
        var mockLoadingPanelService = new Mock<ILoadingPanelService>();
        var service = new MembersLoadService(mockAllMembersInDBService.Object, mockLoadingPanelService.Object);

        // Act
        var result = await service.LoadMembersAsync();

        // Assert
        Assert.Equal(expectedMembers, result);
        mockAllMembersInDBService.Verify(s => s.GetAllMembersInDBAsync(), Times.Once);
    }
}