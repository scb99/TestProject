//using DBExplorerBlazor.Interfaces;
//using DBExplorerBlazor.Services;
//using Moq;
//using Syncfusion.Blazor.Navigations;

//namespace MenuItemPages
//{
//    public class MainLayoutTests
//    {
//        private readonly Mock<IBrowserService> _mockBrowserService;
//        private readonly Mock<IMainLayoutService> _mockMainLayoutService;
//        private readonly Mock<ICrossCuttingLoggedInMemberService> _mockLoggedInMemberService;
//        private readonly MainLayout _mainLayout; // Fix compilation error by specifying the correct type

//        public MainLayoutTests()
//        {
//            _mockBrowserService = new Mock<IBrowserService>();
//            _mockMainLayoutService = new Mock<IMainLayoutService>();
//            _mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();

//            _mainLayout = new MainLayout.MainLayout // Fix compilation error by specifying the correct type
//            {
//                BrowserService = _mockBrowserService.Object,
//                MainLayoutService = _mockMainLayoutService.Object,
//                LoggedInMemberService = _mockLoggedInMemberService.Object
//            };
//        }

//        [Fact]
//        public async Task OnInitializedAsync_SetsPropertiesBasedOnBrowserDimensions()
//        {
//            // Arrange
//            var browserDimension = new BrowserDimension { Width = 1000 };
//            _mockBrowserService.Setup(service => service.GetDimensionsAsync()).ReturnsAsync(browserDimension);

//            // Act
//            await _mainLayout.OnInitializedAsync();

//            // Assert
//            Assert.Equal(1000, _mainLayout.Width);
//            Assert.True(_mainLayout.EnableScrolling);
//            Assert.Equal("e-scrollable-menu", _mainLayout.CSSClass);
//        }

//        [Fact]
//        public async Task OnInitialized2Async_CallsOnInitializedAsync()
//        {
//            // Arrange
//            var browserDimension = new BrowserDimension { Width = 1200 };
//            _mockBrowserService.Setup(service => service.GetDimensionsAsync()).ReturnsAsync(browserDimension);

//            // Act
//            await _mainLayout.OnInitialized2Async();

//            // Assert
//            Assert.Equal(1200, _mainLayout.Width);
//            Assert.False(_mainLayout.EnableScrolling);
//            Assert.Equal(string.Empty, _mainLayout.CSSClass);
//        }

//        [Fact]
//        public async Task OnMenuItemSelectedAsync_CallsHandleMenuItemSelectedAsync()
//        {
//            // Arrange
//            var menuItem = new MenuItem { Text = "Test Menu Item" };
//            var args = new MenuEventArgs<MenuItem> { Item = menuItem };

//            // Act
//            await _mainLayout.OnMenuItemSelectedAsync(args);

//            // Assert
//            _mockMainLayoutService.Verify(service => service.HandleMenuItemSelectedAsync("Test Menu Item"), Times.Once);
//        }
//    }
//}