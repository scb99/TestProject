using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor3TestProject;
using Moq;
using System.Collections.ObjectModel;

namespace MenuItemComponents;

public class PrepareListForClipboardComponentTests
{
    private readonly Mock<ICrossCuttingConditionalCodeService> _mockExecute;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly Mock<IMemberPaymentClipboardService> _mockClipboardService;
    private readonly PrepareListForClipboardComponent _component;

    public PrepareListForClipboardComponentTests()
    {
        _mockExecute = new Mock<ICrossCuttingConditionalCodeService>();
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _mockClipboardService = new Mock<IMemberPaymentClipboardService>();

        _component = new PrepareListForClipboardComponent();

        _component.SetPrivatePropertyValue("Execute", _mockExecute.Object);
        _component.SetPrivatePropertyValue("Logger", _mockLogger.Object);
        _component.SetPrivatePropertyValue("PaymentsService", _mockPaymentsService.Object);
        _component.SetPrivatePropertyValue("MemberPaymentClipboardService", _mockClipboardService.Object);

        typeof(PrepareListForClipboardComponent).Invoke("OnInitialized", _component);
    }

    [Fact]
    public async Task OnClickAsync_ShouldPrepareClipboardList_WhenButtonTitleIsOriginal()
    {
        // Arrange
        _component.SetPrivatePropertyValue<string>("ButtonTitleBDP", _component.GetPrivateMemberValue<string>("_originalButtonTitle"));
        var preparedList = new List<string> { "Item1", "Item2" };
        _mockClipboardService.Setup(s => s.PrepareClipboardList()).Returns(preparedList);

        // Act
        await typeof(PrepareListForClipboardComponent).InvokeAsync("OnClickAsync", _component);

        // Assert
        Assert.Equal(_component.GetPrivateMemberValue<string>("_newButtonTitle"), _component.GetPrivatePropertyValue<string>("ButtonTitleBDP"));
        Assert.Equal(preparedList, _component.GetPrivateMemberValue<List<string>>("_clipBoardList"));
    }

    [Fact]
    public async Task OnClickAsync_ShouldSendNextItemToClipboard_WhenButtonTitleIsNew()
    {
        // Arrange
        _component.SetPrivatePropertyValue<string>("ButtonTitleBDP", _component.GetPrivateMemberValue<string>("_newButtonTitle"));
        _component.SetPrivateMemberValue<List<string>>("_clipBoardList", new List<string> { "Item1", "Item2" } );
        _component.SetPrivateMemberValue<int>("_listIndex", 0);
        _mockClipboardService.Setup(s => s.SendNextItemToClipboardAsync(_component.GetPrivateMemberValue<List<string>>("_clipBoardList"), _component.GetPrivateMemberValue<int>("_listIndex")))
                             .ReturnsAsync(1);

        // Act
        await typeof(PrepareListForClipboardComponent).InvokeAsync("OnClickAsync", _component);

        // Assert
        Assert.Equal(1, _component.GetPrivateMemberValue<int>("_listIndex"));
    }

    [Fact]
    public async Task OnClickAsync_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        _component.SetPrivatePropertyValue<string>("ButtonTitleBDP", _component.GetPrivateMemberValue<string>("_originalButtonTitle"));
        _mockClipboardService.Setup(s => s.PrepareClipboardList()).Throws(new Exception("Test Exception"));

        // Act
        await typeof(PrepareListForClipboardComponent).InvokeAsync("OnClickAsync", _component);

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void OnPaymentEntitiesSizeChanged_ShouldUpdateDisabledBDP()
    {
        // Arrange
        var paymentEntities = new ObservableCollection<PaymentEntity> { new() };
        _mockPaymentsService.Setup(p => p.PaymentEntities).Returns(paymentEntities);
        _mockExecute.Setup(e => e.ConditionalCode()).Returns(false);

        // Act
        typeof(PrepareListForClipboardComponent).Invoke("OnPaymentEntitiesSizeChanged", _component);

        // Assert
        Assert.False(_component.GetPrivatePropertyValue<bool>("DisabledBDP"));
    }

    [Fact]
    public void Dispose_ShouldUnsubscribeFromPaymentEntitiesSizeChanged()
    {
        // Arrange
        var delegateName = typeof(PrepareListForClipboardComponent).GetDelegate("OnPaymentEntitiesSizeChanged", _component);

        // Act
        _component.Dispose();

        // Assert
        _mockPaymentsService.VerifyRemove(p => p.PaymentEntitiesSizeChanged -= (Action)delegateName, Times.Once);
    }
}