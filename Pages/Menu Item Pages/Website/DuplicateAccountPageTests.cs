using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Pages;
using Moq;

namespace MenuItemPages;

public class DuplicateAccountPageTests
{
    private readonly Mock<ICrossCuttingAlertService> _mockShow;
    private readonly Mock<ICrossCuttingAllMembersInDBService> _mockAllMembersInDBService;
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<IRepository<DuplicateAccountEntity>> _mockDuplicateAccountRepository;
    private readonly Mock<IRepositoryDuplicateAccount> _mockDuplicateAccountRepository2;
    private readonly Mock<IRepositoryDuplicateAccountNotToDelete> _mockDuplicateAccountRepository3;
    private readonly Mock<IRepositoryMemberDetail4> _mockMemberDetailRepository;
    private readonly Mock<IRepositoryPaymentDetail> _mockPaymentDetailRepository;
    private readonly Mock<IRepositoryPostmeta2> _mockPostmeta2Repository;
    private readonly Mock<IRepositoryPostmeta4> _mockPostmeta4Repository;
    private readonly Mock<IRepositoryQuickBooks4> _mockQuickBooksRepository4;
    private readonly Mock<IRepositoryUsermeta> _mockUsermetaRepository;
    private readonly Mock<IRepositoryUsers> _mockUsersRepository;
    private readonly DuplicateAccountPage _page;

    public DuplicateAccountPageTests()
    {
        _mockShow = new Mock<ICrossCuttingAlertService>();
        _mockAllMembersInDBService = new Mock<ICrossCuttingAllMembersInDBService>();
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockDuplicateAccountRepository = new Mock<IRepository<DuplicateAccountEntity>>();
        _mockDuplicateAccountRepository2 = new Mock<IRepositoryDuplicateAccount>();
        _mockDuplicateAccountRepository3 = new Mock<IRepositoryDuplicateAccountNotToDelete>();
        _mockMemberDetailRepository = new Mock<IRepositoryMemberDetail4>();
        _mockPaymentDetailRepository = new Mock<IRepositoryPaymentDetail>();
        _mockPostmeta2Repository = new Mock<IRepositoryPostmeta2>();
        _mockPostmeta4Repository = new Mock<IRepositoryPostmeta4>();
        _mockQuickBooksRepository4 = new Mock<IRepositoryQuickBooks4>();
        _mockUsermetaRepository = new Mock<IRepositoryUsermeta>();
        _mockUsersRepository = new Mock<IRepositoryUsers>();

        _page = new DuplicateAccountPage
        {
            Show = _mockShow.Object,
            AllMembersInDBService = _mockAllMembersInDBService.Object,
            Execute = _mockExecute.Object,
            Logger = _mockLogger.Object,
            DuplicateAccountRepository = _mockDuplicateAccountRepository.Object,
            DuplicateAccountRepository2 = _mockDuplicateAccountRepository2.Object,
            DuplicateAccountRepository3 = _mockDuplicateAccountRepository3.Object,
            MemberDetailRepository = _mockMemberDetailRepository.Object,
            PaymentDetailRepository = _mockPaymentDetailRepository.Object,
            Postmeta2Repository = _mockPostmeta2Repository.Object,
            Postmeta4Repository = _mockPostmeta4Repository.Object,
            QuickBooksRepository4 = _mockQuickBooksRepository4.Object,
            UsermetaRepository = _mockUsermetaRepository.Object,
            UsersRepository = _mockUsersRepository.Object
        };
    }

    [Fact]
    public async Task OnParametersSetAsync_PopulatesDuplicateAccountNotToDeleteEntities()
    {
        // Arrange
        var entities = new List<DuplicateAccountNotToDeleteEntity>
        {
            new() { IdNotToDelete = 1 },
            new() { IdNotToDelete = 2 }
        };
        _mockDuplicateAccountRepository3.Setup(repo => repo.GetIDsNotToDeleteAsync()).ReturnsAsync(entities);

        // Act
        await _page.OnParametersSet2Async();

        // Assert
        Assert.Equal(entities, _page.duplicateAccountNotToDeleteEntities);
        Assert.Contains(1, _page.duplicateAccountToDeleteList);
        Assert.Contains(2, _page.duplicateAccountToDeleteList);
    }

    [Fact]
    public async Task OnParametersSetAsync_LogsException()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockDuplicateAccountRepository3.Setup(repo => repo.GetIDsNotToDeleteAsync()).ThrowsAsync(exception);

        // Act
        await _page.OnParametersSet2Async();

        // Assert
        _mockLogger.Verify(logger => logger.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnBlurDuplicateAccountTextBoxAsync_InvalidID_ShowsAlert()
    {
        // Arrange
        DuplicateAccountPage.DuplicateAccountValueBDP = "invalid";

        // Act
        await _page.OnBlurDuplicateAccountTextBoxAsync();

        // Assert
        _mockShow.Verify(show => show.AlertUsingPopUpMessageBoxAsync("ID must be a positive integer!"), Times.Once);
    }

    [Fact]
    public async Task OnBlurDuplicateAccountTextBoxAsync_ValidID_CannotBeDeleted_ShowsAlert()
    {
        // Arrange
        DuplicateAccountPage.DuplicateAccountValueBDP = "1";
        _page.duplicateAccountToDeleteList.Add(1);

        // Act
        await _page.OnBlurDuplicateAccountTextBoxAsync();

        // Assert
        _mockShow.Verify(show => show.AlertUsingPopUpMessageBoxAsync("1 cannot be deleted!"), Times.Once);
    }

    [Fact]
    public async Task OnBlurDuplicateAccountTextBoxAsync_ValidID_ProceedsToStep2()
    {
        // Arrange
        DuplicateAccountPage.DuplicateAccountValueBDP = "1";
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await _page.OnBlurDuplicateAccountTextBoxAsync();

        // Assert
        Assert.Equal(2, DuplicateAccountPage.StepNumber);
    }

    [Fact]
    public async Task OnBlurExistingAccountTextBoxAsync_InvalidID_ShowsAlert()
    {
        // Arrange
        DuplicateAccountPage.ExistingAccountValueBDP = "invalid";

        // Act
        await _page.OnBlurExistingAccountTextBoxAsync();

        // Assert
        _mockShow.Verify(show => show.AlertUsingPopUpMessageBoxAsync("ID must be a positive integer!"), Times.Once);
    }

    [Fact]
    public async Task OnBlurExistingAccountTextBoxAsync_ValidID_CannotBeDeleted_ShowsAlert()
    {
        // Arrange
        DuplicateAccountPage.ExistingAccountValueBDP = "1";
        _page.duplicateAccountToDeleteList.Add(1);

        // Act
        await _page.OnBlurExistingAccountTextBoxAsync();

        // Assert
        _mockShow.Verify(show => show.AlertUsingPopUpMessageBoxAsync("1 cannot be deleted!"), Times.Once);
    }

    [Fact]
    public async Task OnBlurExistingAccountTextBoxAsync_SameAsDuplicateID_ShowsAlert()
    {
        // Arrange
        DuplicateAccountPage.ExistingAccountValueBDP = "1";
        _page.duplicateUserID = 1;

        // Act
        await _page.OnBlurExistingAccountTextBoxAsync();

        // Assert
        _mockShow.Verify(show => show.AlertUsingPopUpMessageBoxAsync("Duplicate Account ID and Existing Account ID cannot be the same!"), Times.Once);
    }

    [Fact]
    public async Task OnBlurExistingAccountTextBoxAsync_ValidID_ProceedsToStep3()
    {
        // Arrange
        DuplicateAccountPage.ExistingAccountValueBDP = "2";
        _page.duplicateUserID = 1;
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        await _page.OnBlurExistingAccountTextBoxAsync();

        // Assert
        Assert.Equal(3, DuplicateAccountPage.StepNumber);
    }

    // Additional tests for other methods can be added similarly...
}