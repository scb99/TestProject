using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace CrossCuttingConcerns;

public class FileNameValidationServiceTests
{
    [Fact]
    public async Task ValidateAndAlertAsync_ReturnsTrue_WhenFileNameIsValid()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        mockIsValidFileNameService.Setup(service => service.FileNameValid(It.IsAny<string>())).Returns(true);

        var fileNameValidationService = new FileNameValidationService(mockAlertService.Object, mockIsValidFileNameService.Object);
        string validFileName = "ValidFileName.xlsx";

        // Act
        var result = await fileNameValidationService.ValidateAndAlertAsync(validFileName);

        // Assert
        Assert.True(result);
        mockAlertService.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ValidateAndAlertAsync_ReturnsFalse_WhenFileNameIsInvalid()
    {
        // Arrange
        var mockAlertService = new Mock<ICrossCuttingAlertService>();
        var mockIsValidFileNameService = new Mock<ICrossCuttingIsValidFileNameService>();
        mockIsValidFileNameService.Setup(service => service.FileNameValid(It.IsAny<string>())).Returns(false);

        var fileNameValidationService = new FileNameValidationService(mockAlertService.Object, mockIsValidFileNameService.Object);
        string invalidFileName = "Invalid/FileName.xlsx";

        // Act
        var result = await fileNameValidationService.ValidateAndAlertAsync(invalidFileName);

        // Assert
        Assert.False(result);
        mockAlertService.Verify(service => service.InappropriateFileNameAlertUsingFallingMessageBoxAsync(invalidFileName), Times.Once);
    }
}