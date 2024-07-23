using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Service;

public class DBOperationServiceTests
{
    private readonly Mock<IAlertService> _alertServiceMock;
    private readonly DBOperationService _dbOperationService;

    public DBOperationServiceTests()
    {
        _alertServiceMock = new Mock<IAlertService>();
        _dbOperationService = new DBOperationService();
    }

    [Theory]
    [InlineData(DBOperation.Create, "New record successfully created in DB!")]
    [InlineData(DBOperation.Delete, "Selected record successfully deleted from DB!")]
    [InlineData(DBOperation.Update, "Selected record successfully updated in DB!")]
    public async Task AfterSuccessfulDBOperationAsync_DisplaysCorrectMessage(DBOperation operation, string expectedMessage)
    {
        // Act
        await _dbOperationService.AfterSuccessfulDBOperationAsync(operation, _alertServiceMock.Object);

        // Assert
        _alertServiceMock.Verify(a => a.AlertUsingFallingMessageBoxAsync(expectedMessage), Times.Once);
    }

    [Theory]
    [InlineData(DBOperation.Create, "New record was NOT successfully created in DB!")]
    [InlineData(DBOperation.Read, "Tried to read record and got message:")]
    [InlineData(DBOperation.Delete, "Selected record was NOT deleted from DB!")]
    [InlineData(DBOperation.Update, "Selected record was NOT updated in DB!")]
    public async Task AfterUnSuccessfulDBOperationAsync_DisplaysCorrectMessage(DBOperation operation, string expectedMessage)
    {
        // Arrange
        string errorMessage = "";
        string fullErrorMessage = operation == DBOperation.Read ? expectedMessage + errorMessage : expectedMessage;

        // Act
        await _dbOperationService.AfterUnSuccessfulDBOperationAsync(operation, _alertServiceMock.Object, errorMessage);

        // Assert
        _alertServiceMock.Verify(a => a.AlertUsingFallingMessageBoxAsync(fullErrorMessage), Times.Once);
    }
}