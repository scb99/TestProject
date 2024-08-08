//using DBExplorerBlazor.Services;
//using Microsoft.AspNetCore.Components;
//using Moq;

//namespace CrossCuttingConcerns;

//public class NavigationServiceTests
//{
//    [Fact]
//    public void NavigateTo_CallsNavigationManagerNavigateTo()
//    {
//        // Arrange
//        var mockNavigationManager = new Mock<NavigationManager>();
//        var navigationService = new NavigationService(mockNavigationManager.Object);
//        var url = "test-url";

//        // Act
//        navigationService.NavigateTo(url);

//        // Assert
//        mockNavigationManager.Verify(nm => nm.NavigateTo(url, It.IsAny<bool>()), Times.Once);
//    }
//}