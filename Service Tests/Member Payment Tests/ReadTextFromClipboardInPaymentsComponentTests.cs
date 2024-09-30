﻿using DataAccess.Models;
using DBExplorerBlazor.Components;
using DBExplorerBlazor.Interfaces;
using Microsoft.JSInterop;
using Moq;

namespace MemberPayment;

public class ReadTextFromClipboardInPaymentsComponentTests
{
    private readonly Mock<ICrossCuttingAllMembersInDBService> _allMembersInDBServiceMock;
    private readonly Mock<ICrossCuttingAlertService> _showMock;
    private readonly Mock<ICrossCuttingLoggerService> _loggerMock;
    private readonly Mock<ICrossCuttingPaymentsService> _paymentsServiceMock;
    private readonly Mock<IJSRuntime> _jsRuntimeMock;
    private readonly Mock<IMemberPaymentClipboardTextProcessorService> _memberPaymentClipboardTextProcessorServiceMock;
    private readonly ReadTextFromClipboardInPaymentsComponent _component;

    public ReadTextFromClipboardInPaymentsComponentTests()
    {
        _allMembersInDBServiceMock = new Mock<ICrossCuttingAllMembersInDBService>();
        _showMock = new Mock<ICrossCuttingAlertService>();
        _loggerMock = new Mock<ICrossCuttingLoggerService>();
        _paymentsServiceMock = new Mock<ICrossCuttingPaymentsService>();
        _jsRuntimeMock = new Mock<IJSRuntime>();
        _memberPaymentClipboardTextProcessorServiceMock = new Mock<IMemberPaymentClipboardTextProcessorService>();

        _component = new ReadTextFromClipboardInPaymentsComponent
        {
            AllMembersInDBService = _allMembersInDBServiceMock.Object,
            Show = _showMock.Object,
            Logger = _loggerMock.Object,
            PaymentsService = _paymentsServiceMock.Object,
            JS = _jsRuntimeMock.Object,
            MemberPaymentClipboardTextProcessorService = _memberPaymentClipboardTextProcessorServiceMock.Object
        };
    }

    [Fact]
    public async Task OnReadTextFromClipboardButtonClickedAsync_ValidText_UpdatesPaymentsGrid()
    {
        // Arrange
        var textFromClipboard = "1 R 100\n2 D 200";
        var paymentEntities = new List<PaymentEntity>
        {
            new() { ID = 1, LastName = "Doe", FirstName = "John", Description = "R", Amount = "100" },
            new() { ID = 2, LastName = "Smith", FirstName = "Jane", Description = "D", Amount = "200" }
        };

        _memberPaymentClipboardTextProcessorServiceMock.Setup(m => m.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object))
            .Returns(paymentEntities);

        // Act
        await _component.OnReadTextFromClipboardButtonClickedAsync(textFromClipboard);

        // Assert
        _paymentsServiceMock.Verify(m => m.RemoveAllPaymentsFromPaymentEntities(), Times.Once);
        _paymentsServiceMock.Verify(m => m.AddPaymentToPaymentEntities(It.IsAny<PaymentEntity>()), Times.Exactly(paymentEntities.Count));
    }

    [Fact]
    public async Task OnReadTextFromClipboardButtonClickedAsync_InvalidText_LogsException()
    {
        // Arrange
        var textFromClipboard = "invalid text";
        var exception = new Exception("Test exception");

        _memberPaymentClipboardTextProcessorServiceMock.Setup(m => m.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object))
            .Throws(exception);

        // Act
        await _component.OnReadTextFromClipboardButtonClickedAsync(textFromClipboard);

        // Assert
        _loggerMock.Verify(m => m.LogExceptionAsync(exception, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task OnReadTextFromClipboardButtonClickedAsync_EmptyText_DoesNotUpdatePaymentsGrid()
    {
        // Arrange
        var textFromClipboard = string.Empty;

        _memberPaymentClipboardTextProcessorServiceMock.Setup(m => m.ProcessText(textFromClipboard, _allMembersInDBServiceMock.Object))
            .Returns((List<PaymentEntity>)null);

        // Act
        await _component.OnReadTextFromClipboardButtonClickedAsync(textFromClipboard);

        // Assert
        _paymentsServiceMock.Verify(m => m.RemoveAllPaymentsFromPaymentEntities(), Times.Never);
        _paymentsServiceMock.Verify(m => m.AddPaymentToPaymentEntities(It.IsAny<PaymentEntity>()), Times.Never);
    }
}