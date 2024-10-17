using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace MembersList;

public class MembersListComboBoxProcessingServiceTests
{
    private readonly Mock<IMembersListMemberFilteringService> _memberFilteringServiceMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly MembersListComboBoxProcessingService _service;

    public MembersListComboBoxProcessingServiceTests()
    {
        _memberFilteringServiceMock = new Mock<IMembersListMemberFilteringService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _service = new MembersListComboBoxProcessingService(_memberFilteringServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ProcessComboBoxChangeAsync_CallsFilterMembersAsyncWithCorrectParameters()
    {
        // Arrange
        var comboBoxValue = "Filter by Last Name";
        var members = new List<MemberEntity> { new MemberEntity() };
        _memberFilteringServiceMock.Setup(s => s.FilterMembersAsync(comboBoxValue, string.Empty)).ReturnsAsync(members);

        // Act
        var result = await _service.ProcessComboBoxChangeAsync(comboBoxValue);

        // Assert
        _memberFilteringServiceMock.Verify(s => s.FilterMembersAsync(comboBoxValue, string.Empty), Times.Once);
        Assert.Equal(members, result);
    }

    [Fact]
    public async Task ProcessComboBoxChangeAsync_LogsCorrectMessage()
    {
        // Arrange
        var comboBoxValue = "Filter by Last Name";
        var members = new List<MemberEntity> { new MemberEntity() };
        _memberFilteringServiceMock.Setup(s => s.FilterMembersAsync(comboBoxValue, string.Empty)).ReturnsAsync(members);

        // Act
        await _service.ProcessComboBoxChangeAsync(comboBoxValue);

        // Assert
        _loggerMock.Verify(s => s.LogResultAsync($"ComboBox value changed to: {comboBoxValue}"), Times.Once);
    }

    [Fact]
    public async Task ProcessComboBoxChangeAsync_ReturnsCorrectResult()
    {
        // Arrange
        var comboBoxValue = "Filter by Last Name";
        var members = new List<MemberEntity> { new MemberEntity() };
        _memberFilteringServiceMock.Setup(s => s.FilterMembersAsync(comboBoxValue, string.Empty)).ReturnsAsync(members);

        // Act
        var result = await _service.ProcessComboBoxChangeAsync(comboBoxValue);

        // Assert
        Assert.Equal(members, result);
    }
}