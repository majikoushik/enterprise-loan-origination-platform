using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Worker.Controllers;
using Notification.Worker.Infrastructure.Data;
using Xunit;

namespace Notification.Worker.Tests;

public class InternalEventsControllerTests
{
    private readonly DbContextOptions<NotificationDbContext> _dbContextOptions;

    public InternalEventsControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task ReceiveEvent_ValidLoanApplicationStatusChangedEvent_ShouldCreateNotificationRequest()
    {
        // Arrange
        using var dbContext = new NotificationDbContext(_dbContextOptions);
        var loggerMock = new Mock<ILogger<InternalEventsController>>();
        var controller = new InternalEventsController(dbContext, loggerMock.Object);

        var eventJson = @"
        {
            ""eventType"": ""LoanApplicationStatusChanged"",
            ""correlationId"": ""test-correlation-123"",
            ""entityId"": ""12345678-1234-1234-1234-123456789012"",
            ""newStatus"": ""UnderReview"",
            ""customerId"": ""87654321-4321-4321-4321-210987654321""
        }";
        var jsonDoc = JsonDocument.Parse(eventJson);

        // Act
        var result = await controller.ReceiveEvent(jsonDoc.RootElement);

        // Assert
        result.Should().BeOfType<OkResult>();

        var requests = await dbContext.NotificationRequests.ToListAsync();
        requests.Should().HaveCount(1);
        
        var request = requests[0];
        request.EventType.Should().Be("LoanApplicationStatusChanged");
        request.CorrelationId.Should().Be("test-correlation-123");
        request.Subject.Should().Contain("UnderReview");
        request.Status.Should().Be(Domain.Models.NotificationStatus.Pending);
    }

    [Fact]
    public async Task ReceiveEvent_InvalidPayload_ShouldReturnBadRequest()
    {
        // Arrange
        using var dbContext = new NotificationDbContext(_dbContextOptions);
        var loggerMock = new Mock<ILogger<InternalEventsController>>();
        var controller = new InternalEventsController(dbContext, loggerMock.Object);

        var eventJson = @"{ ""wrongField"": ""value"" }"; // Missing eventType which will throw Exception
        var jsonDoc = JsonDocument.Parse(eventJson);

        // Act
        var result = await controller.ReceiveEvent(jsonDoc.RootElement);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
