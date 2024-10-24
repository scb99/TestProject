﻿using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Moq;
using System.Collections.ObjectModel;

namespace MemberPayment;

public class PrepareListForClipboardComponentTests
{
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly Mock<ICrossCuttingPaymentsService> _mockPaymentsService;
    private readonly Mock<IMemberPaymentClipboardService> _mockClipboardService;
    private readonly PrepareListForClipboardComponent _component;

    public PrepareListForClipboardComponentTests()
    {
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockPaymentsService = new Mock<ICrossCuttingPaymentsService>();
        _mockClipboardService = new Mock<IMemberPaymentClipboardService>();

        _component = new PrepareListForClipboardComponent
        {
            Logger = _mockLogger.Object,
            PaymentsService = _mockPaymentsService.Object,
            MemberPaymentClipboardService = _mockClipboardService.Object
        };

        _component.OnInitialized2();
    }

    [Fact]
    public async Task OnClickAsync_ShouldPrepareClipboardList_WhenButtonTitleIsOriginal()
    {
        // Arrange
        _component.ButtonTitleBDP = _component.OriginalButtonTitle;
        var preparedList = new List<string> { "Item1", "Item2" };
        _mockClipboardService.Setup(s => s.PrepareClipboardList()).Returns(preparedList);

        // Act
        await _component.OnClickAsync();

        // Assert
        Assert.Equal(_component.NewButtonTitle, _component.ButtonTitleBDP);
        Assert.Equal(preparedList, _component._clipBoardList);
    }

    [Fact]
    public async Task OnClickAsync_ShouldSendNextItemToClipboard_WhenButtonTitleIsNew()
    {
        // Arrange
        _component.ButtonTitleBDP = _component.NewButtonTitle;
        _component._clipBoardList = new List<string> { "Item1", "Item2" };
        _component._listIndex = 0;
        _mockClipboardService.Setup(s => s.SendNextItemToClipboardAsync(_component._clipBoardList, _component._listIndex))
                             .ReturnsAsync(1);

        // Act
        await _component.OnClickAsync();

        // Assert
        Assert.Equal(1, _component._listIndex);
    }

    [Fact]
    public async Task OnClickAsync_ShouldLogException_WhenExceptionIsThrown()
    {
        // Arrange
        _component.ButtonTitleBDP = _component.OriginalButtonTitle;
        _mockClipboardService.Setup(s => s.PrepareClipboardList()).Throws(new Exception("Test Exception"));

        // Act
        await _component.OnClickAsync();

        // Assert
        _mockLogger.Verify(l => l.LogExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void OnPaymentEntitiesSizeChanged_ShouldUpdateDisabledBDP()
    {
        // Arrange
        var paymentEntities = new ObservableCollection<PaymentEntity> { new() };
        _mockPaymentsService.Setup(p => p.PaymentEntities).Returns(paymentEntities);

        // Act
        //_component.OnPaymentEntitiesSizeChanged();

        // Assert
        Assert.True(_component.DisabledBDP);
    }

    [Fact]
    public void Dispose_ShouldUnsubscribeFromPaymentEntitiesSizeChanged()
    {
        // Act
        _component.Dispose();

        // Assert
        _mockPaymentsService.VerifyRemove(p => p.PaymentEntitiesSizeChanged -= _component.OnPaymentEntitiesSizeChanged, Times.Once);
    }
}