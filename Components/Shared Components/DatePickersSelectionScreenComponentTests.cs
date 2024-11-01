using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;

namespace SharedComponents;

public class DatePickersSelectionScreenComponentTests
{
    [Fact]
    public void OnInitialized_ShouldSetStartDateAndEndDate()
    {
        // Arrange
        var component = new DatePickersSelectionScreenComponent();

        // Act
        component.OnInitialized2();

        // Assert
        Assert.Equal(DateTime.Now.Date, component.StartDate);
        Assert.Equal(DateTime.Now.Date, component.EndDate);
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldPreventStartDateBeingGreaterThanEndDate()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        var component = new DatePickersSelectionScreenComponent
        {
            Show = mockShow.Object,
            Execute = mockExecute.Object
        };
        component.Initialize(new DateTime(2023, 1, 2), new DateTime(2023, 1, 1), new EventCallback<DateTime>(), new EventCallback<DateTime>());
        mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        Assert.Equal(new DateTime(2023, 1, 2), component.StartDate);
        Assert.Equal(new DateTime(2023, 1, 1), component.EndDate);
        mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync($"You cannot have a start date later than the end date!{Environment.NewLine} We will reset end date to its previous value."), Times.Once);
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldInvokeStartDateChangedAndEndDateChangedIfDatesChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var startDateChanged = new EventCallback<DateTime>();
        var endDateChanged = new EventCallback<DateTime>();

        var component = new DatePickersSelectionScreenComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
        };
        component.Initialize(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2), startDateChanged, endDateChanged);

        var previousStartDateField = component.GetType().GetField("_previousStartDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousStartDateField?.SetValue(component, new DateTime(2022, 1, 1));

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2022, 1, 2));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1), previousStartDateField?.GetValue(component));
        Assert.Equal(new DateTime(2022, 1, 2), previousEndDateField?.GetValue(component));
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldShowAlertIfDatesNotChanged()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var component = new DatePickersSelectionScreenComponent
        {
            Show = mockShow.Object,
        };
        component.Initialize(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), new EventCallback<DateTime>(), new EventCallback<DateTime>());

        var previousStartDateField = component.GetType().GetField("_previousStartDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousStartDateField?.SetValue(component, new DateTime(2023, 1, 1));

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2023, 1, 1));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        mockShow.Verify(s => s.AlertUsingPopUpMessageBoxAsync("You didn't change either date!"), Times.Once);
    }

    [Fact]
    public async Task OnSubmitButtonClickedAsync_ShouldLogExceptionOnFailure()
    {
        // Arrange
        var mockShow = new Mock<ICrossCuttingAlertService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var component = new DatePickersSelectionScreenComponent
        {
            Show = mockShow.Object,
            Logger = mockLogger.Object,
        };
        component.Initialize(new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), new EventCallback<DateTime>(), new EventCallback<DateTime>());

        var previousStartDateField = component.GetType().GetField("_previousStartDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousStartDateField?.SetValue(component, new DateTime(2023, 1, 1));

        var previousEndDateField = component.GetType().GetField("_previousEndDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        previousEndDateField?.SetValue(component, new DateTime(2023, 1, 1));

        mockShow.Setup(s => s.AlertUsingPopUpMessageBoxAsync(It.IsAny<string>())).Throws(new Exception("Test Exception"));

        // Act
        await component.OnSubmitButtonClickedAsync();

        // Assert
        mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}