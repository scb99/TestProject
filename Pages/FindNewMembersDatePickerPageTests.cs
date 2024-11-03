using DBExplorerBlazor.Pages;

namespace Pages;

public class FindNewMembersDatePickerPageTests
{
    [Fact]
    public void EndDateBDP_ShouldBeInitializedToMinValue()
    {
        // Arrange
        var page = new FindNewMembersDatePickerPage();

        // Act
        var endDate = page.EndDateBDP;

        // Assert
        Assert.Equal(DateTime.MinValue, endDate);
    }

    [Fact]
    public void StartDateBDP_ShouldBeInitializedToMinValue()
    {
        // Arrange
        var page = new FindNewMembersDatePickerPage();

        // Act
        var startDate = page.StartDateBDP;

        // Assert
        Assert.Equal(DateTime.MinValue, startDate);
    }

    [Fact]
    public void CanShowListOfNewMembers_ShouldReturnFalse_WhenBothDatesAreMinValue()
    {
        // Arrange
        var page = new FindNewMembersDatePickerPage();

        // Act
        var canShowList = page.CanShowListOfNewMembers;

        // Assert
        Assert.False(canShowList);
    }

    [Fact]
    public void CanShowListOfNewMembers_ShouldReturnTrue_WhenEndDateIsNotMinValue()
    {
        // Arrange
        var page = new FindNewMembersDatePickerPage
        {
            EndDateBDP = DateTime.Now
        };

        // Act
        var canShowList = page.CanShowListOfNewMembers;

        // Assert
        Assert.True(canShowList);
    }

    [Fact]
    public void CanShowListOfNewMembers_ShouldReturnTrue_WhenStartDateIsNotMinValue()
    {
        // Arrange
        var page = new FindNewMembersDatePickerPage
        {
            StartDateBDP = DateTime.Now
        };

        // Act
        var canShowList = page.CanShowListOfNewMembers;

        // Assert
        Assert.True(canShowList);
    }
}