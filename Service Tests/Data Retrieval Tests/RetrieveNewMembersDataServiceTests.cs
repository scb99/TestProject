﻿using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace DataRetrieval;

public class RetrieveNewMembersDataServiceTests
{
    private readonly Mock<IRepositoryNewMembers> _mockDataManager;
    private readonly Mock<ICrossCuttingLoggerService> _mockLogger;
    private readonly RetrieveNewMembersDataService _service;

    public RetrieveNewMembersDataServiceTests()
    {
        _mockLogger = new Mock<ICrossCuttingLoggerService>();
        _mockDataManager = new Mock<IRepositoryNewMembers>();
        _service = new RetrieveNewMembersDataService(_mockLogger.Object, _mockDataManager.Object);
    }

    [Fact]
    public async Task FetchNewMembersDataAsync_ShouldReturnNewMembers()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var newMembers = new List<NewMemberEntity> { new() };

        _mockDataManager.Setup(dm => dm.GetNewMembersAsync(startDate, endDate))
            .ReturnsAsync(newMembers);

        // Act
        var result = await _service.RetrieveNewMembersDataAsync(startDate, endDate);

        // Assert
        Assert.Equal(newMembers, result);
        _mockLogger.Verify(l => l.LogResultAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task FetchNewMembersDataAsync_ShouldLogException_WhenExceptionThrown()
    {
        // Arrange
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);
        var exception = new Exception("Test exception");

        _mockDataManager.Setup(dm => dm.GetNewMembersAsync(startDate, endDate))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.RetrieveNewMembersDataAsync(startDate, endDate));
        Assert.Equal(exception, ex);
        _mockLogger.Verify(l => l.LogExceptionAsync(exception, nameof(_service.RetrieveNewMembersDataAsync)), Times.Once);
    }
}