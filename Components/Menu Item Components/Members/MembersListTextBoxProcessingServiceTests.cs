using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MembersListTextBoxProcessingServiceTests
{
    [Fact]
    public async Task ProcessTextBoxInputChangeAsync_ReturnsFilteredMembers()
    {
        // Arrange
        var mockMemberFilteringService = new Mock<IMembersListMemberFilteringService>();
        var testValue = "test";
        var testFilterCriteria = "criteria";
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };

        mockMemberFilteringService.Setup(service => service.FilterMembersAsync(testValue, testFilterCriteria))
            .ReturnsAsync(expectedMembers);

        var textBoxProcessService = new MembersListTextBoxProcessingService(mockMemberFilteringService.Object);

        // Act
        var result = await textBoxProcessService.ProcessTextBoxInputChangeAsync(testValue, testFilterCriteria);

        // Assert
        Assert.Equal(expectedMembers.Count, result.Count);
        mockMemberFilteringService.Verify(service => service.FilterMembersAsync(testValue, testFilterCriteria), Times.Once);
    }
}
