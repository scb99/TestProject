using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;

namespace SharedComponents;

public class DatePickerSelectionScreenComponentTests
{
    [Fact]
    public void OnInitialized_ShouldSetEndDateAndPreviousEndDate()
    {
        // Arrange
        var mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        mockSystemTimeService.Setup(s => s.Now).Returns(new DateTime(2023, 1, 1));
        mockSystemTimeService.Setup(s => s.MinValue).Returns(new DateTime(2022, 1, 1));

        var component = new DatePickerSelectionScreenComponent
        {
            SystemTimeService = mockSystemTimeService.Object
        };

        // Act
        component.OnInitialized2();

        // Assert
        Assert.Equal(new DateTime(2023, 1, 1), component.EndDate);
        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var previousEndDateValue = previousEndDateField?.GetValue(component) as DateTime?;
        Assert.Equal(new DateTime(2022, 1, 1), previousEndDateValue);
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldInvokeEndDateChangedIfDateChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        var endDateChanged = new EventCallback<DateTime>();

        var component = new DatePickerSelectionScreenComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
            SystemTimeService = mockSystemTimeService.Object,
        };
        component.Initialize(new DateTime(2023, 1, 1), endDateChanged);

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2022, 1, 1));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        Assert.Equal(new DateTime(2023, 1, 1), previousEndDateField?.GetValue(component));
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldShowAlertIfDateNotChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        var endDateChanged = new EventCallback<DateTime>();

        var component = new DatePickerSelectionScreenComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
            SystemTimeService = mockSystemTimeService.Object,
        };
        component.Initialize(new DateTime(2023, 1, 1), endDateChanged);

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2023, 1, 1));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("You didn't change the date!"), Times.Once);
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockSystemTimeService = new Mock<ICrossCuttingSystemTimeService>();
        var endDateChanged = new EventCallback<DateTime>();

        var component = new DatePickerSelectionScreenComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
            SystemTimeService = mockSystemTimeService.Object,
        };
        component.Initialize(new DateTime(2023, 1, 1), endDateChanged);

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2023, 1, 1));

        mockShow.Setup(s => s.AlertUsingPopUpMessageBoxAsync(It.IsAny<string>())).Throws(new Exception("Test Exception"));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}