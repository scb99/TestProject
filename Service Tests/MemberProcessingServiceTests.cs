using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class MemberProcessingServiceTests
{
    [Fact]
    public async Task ProcessComboBoxChangeAsync_CallsFilterMembersAsync_WithCorrectParameters()
    {
        // Arrange
        var mockFilteringService = new Mock<IMemberFilteringService>();
        var service = new MemberProcessingService(mockFilteringService.Object);
        string comboBoxValue = "TestValue";
        var expectedMembers = new List<MemberEntity>
        {
            new() { /* Initialize properties as needed */ },
            new() { /* Initialize properties as needed */ }
        };

        mockFilteringService.Setup(s => s.FilterMembersAsync(comboBoxValue, string.Empty))
            .ReturnsAsync(expectedMembers)
            .Verifiable("FilterMembersAsync was not called with the expected parameters.");

        // Act
        var result = await service.ProcessComboBoxChangeAsync(comboBoxValue);

        // Assert
        mockFilteringService.Verify(); // Verify that FilterMembersAsync was called with the correct parameters
        Assert.Equal(expectedMembers, result); // Optionally verify that the result matches the expected output
    }
}