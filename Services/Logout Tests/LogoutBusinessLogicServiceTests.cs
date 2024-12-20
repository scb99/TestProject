﻿using DataAccess.Interfaces;
using DataAccess.Models;
using DBExplorerBlazor.Interfaces;
using DBExplorerBlazor.Services;
using Moq;

namespace Logout;

public class LogoutBusinessLogicServiceTests
{
    [Fact]
    public async Task CanLogoutAsync_SuperUserWithMultipleActiveRecords_ReturnsFalse()
    {
        // Arrange
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        var mockGetMembersWithMultipleActiveRecords = new Mock<IRepositoryGetMembersWith<MemberEntity>>();

        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");
        mockGetMembersWithMultipleActiveRecords.Setup(manager => manager.GetMembersWithMultipleActiveRecordsAsync())
            .ReturnsAsync(new List<MemberEntity> { new(), new() });

        var service = new LogoutBusinessLogicService(
            mockLoggedInMemberService.Object,
            mockLogger.Object,
            mockMemberIDService.Object,
            mockMemberNameService.Object,
            mockGetMembersWithMultipleActiveRecords.Object
        );

        // Act
        var result = await service.CanLogoutAsync();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanLogoutAsync_SuperUserWithNoMultipleActiveRecords_ReturnsTrue()
    {
        // Arrange
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        var mockGetMembersWithMultipleActiveRecords = new Mock<IRepositoryGetMembersWith<MemberEntity>>();

        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("SuperUser");
        mockGetMembersWithMultipleActiveRecords.Setup(manager => manager.GetMembersWithMultipleActiveRecordsAsync())
            .ReturnsAsync(new List<MemberEntity>());

        var service = new LogoutBusinessLogicService(
            mockLoggedInMemberService.Object,
            mockLogger.Object,
            mockMemberIDService.Object,
            mockMemberNameService.Object,
            mockGetMembersWithMultipleActiveRecords.Object
        );

        // Act
        var result = await service.CanLogoutAsync();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanLogoutAsync_NonSuperUser_ReturnsTrue()
    {
        // Arrange
        var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
        var mockLogger = new Mock<ICrossCuttingLoggerService>();
        var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
        var mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();
        var mockGetMembersWithMultipleActiveRecords = new Mock<IRepositoryGetMembersWith<MemberEntity>>();

        mockLoggedInMemberService.Setup(service => service.MemberRole).Returns("RegularUser");

        var service = new LogoutBusinessLogicService(
            mockLoggedInMemberService.Object,
            mockLogger.Object,
            mockMemberIDService.Object,
            mockMemberNameService.Object,
            mockGetMembersWithMultipleActiveRecords.Object
        );

        // Act
        var result = await service.CanLogoutAsync();

        // Assert
        Assert.True(result);
    }

    //[Fact]
    //public async Task PerformLogoutAsync_LogsOutAndResetsMemberInfo()
    //{
    //    // Arrange
    //    var mockDataManager = new Mock<IDataManager>();
    //    var mockLoggedInMemberService = new Mock<ICrossCuttingLoggedInMemberService>();
    //    var mockLogger = new Mock<ICrossCuttingLoggerService>();
    //    var mockMemberIDService = new Mock<ICrossCuttingMemberIDService>();
    //    var _mockMemberNameService = new Mock<ICrossCuttingMemberNameService>();

    //    var service = new LogoutBusinessLogicService(
    //        mockDataManager.Object,
    //        mockLoggedInMemberService.Object,
    //        mockLogger.Object,
    //        mockMemberIDService.Object,
    //        _mockMemberNameService.Object
    //    );

    //    // Act
    //    await service.PerformLogoutAsync();

    //    // Assert
    //    mockLogger.Verify(logger => logger.LogResultAsync("Logged out"), Times.Once);
    //    Assert.Equal(0, mockLoggedInMemberService.Object.MemberUserID);
    //    Assert.Equal("<No Member Selected>", _mockMemberNameService.Object.MemberName);
    //    Assert.Equal(0, mockMemberIDService.Object.MemberID);
    //}
}