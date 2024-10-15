using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.JSInterop;
using Moq;

namespace MenuItemComponents;

public class CompleteRosterComponentTests
{
    private readonly CompleteRosterComponent _component;
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<IJSRuntime> _mockJS;
    private readonly Mock<ICrossCuttingLoadingPanelService> _mockLoadingPanelService;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IRepositoryCompleteRoster> _mockRosterRepository;

    public CompleteRosterComponentTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockJS = new Mock<IJSRuntime>();
        _mockLoadingPanelService = new Mock<ICrossCuttingLoadingPanelService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockRosterRepository = new Mock<IRepositoryCompleteRoster>();

        _component = new CompleteRosterComponent
        {
            Show = _mockShow.Object,
            JS = _mockJS.Object,
            LoadingPanelService = _mockLoadingPanelService.Object,
            Logger = _mockLogger.Object,
            RosterRepository = _mockRosterRepository.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_LoadsCompleteRosterAndSetsTitle()
    {
        // Arrange
        var completeRosterEntities = new List<CompleteRosterEntity>
        {
            new() { FirstName = "John", LastName = "Doe", EmailAddress = "doe@gmail.com" },
            new() { FirstName = "Jane", LastName = "Smith", EmailAddress = "smith@gmail.com" }
        };
        _mockRosterRepository
            .Setup(repo => repo.GetCompleteRosterAsync(It.IsAny<int>()))
            .ReturnsAsync(completeRosterEntities);
        _component.GracePeriod = 30;

        // Act
        await _component.OnParametersSet2Async();

        // Assert
        Assert.Equal(completeRosterEntities, _component.CompleteRosterEntities);
        Assert.Equal("2 members in complete roster with grace period of 30 days", _component.TitleBDP);
        //_mockJS.Verify(js => js.InvokeVoidAsync("navigator.clipboard.writeText", It.IsAny<object[]>()), Times.Once);
        _mockShow.Verify(show => show.AlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Once);
        _mockLogger.Verify(logger => logger.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void ProduceCompleteRosterLines_GeneratesCorrectLines()
    {
        // Arrange
        var completeRosterEntities = new List<CompleteRosterEntity>
        {
            new() { FirstName = "John", LastName = "Doe", Address1 = "123 Main St", City = "Anytown", State = "CA", Zip = "12345", Gender = "M", Rating = "A", HomePhone = "123-456-7890", OtherPhone = "098-765-4321", EmailAddress = "john.doe@example.com", RenewalDate = "2023-12-31" },
            new() { FirstName = "Jane", LastName = "Smith", Address1 = "456 Elm St", City = "Othertown", State = "TX", Zip = "67890", Gender = "F", Rating = "B", HomePhone = "234-567-8901", OtherPhone = "987-654-3210", EmailAddress = "jane.smith@example.com", RenewalDate = "2023-12-31" }
        };

        // Act
        var result = _component.ProduceCompleteRosterLines(completeRosterEntities);

        // Assert
        Assert.Contains("Doe, John", result);
        Assert.Contains("Smith, Jane", result);
        Assert.Contains("123 Main St", result);
        Assert.Contains("456 Elm St", result);
        Assert.Contains("john.doe@example.com", result);
        Assert.Contains("jane.smith@example.com", result);
    }
}