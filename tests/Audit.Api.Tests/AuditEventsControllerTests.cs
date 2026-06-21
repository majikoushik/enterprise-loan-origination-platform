using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using Audit.Api.Controllers;
using Audit.Api.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel;
using Xunit;

namespace Audit.Api.Tests;

public class AuditEventsControllerTests
{
    private readonly DbContextOptions<AuditDbContext> _dbContextOptions;

    public AuditEventsControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AuditDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task IngestEvent_ValidEvent_ShouldSaveToDatabase()
    {
        // Arrange
        using var dbContext = new AuditDbContext(_dbContextOptions);
        var loggerMock = new Mock<ILogger<AuditEventsController>>();
        var controller = new AuditEventsController(dbContext, loggerMock.Object);

        var eventRecord = new AuditEventRecord(
            Guid.NewGuid(),
            "corr-123",
            "CustomerRegistered",
            "Customer",
            "Customer",
            "123",
            Guid.NewGuid(),
            "System",
            "System",
            "Register",
            "Summary",
            "{}",
            DateTimeOffset.UtcNow,
            "Customer.Api",
            "Info"
        );

        // Act
        var result = await controller.IngestEvent(eventRecord, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>();

        var savedEvents = await dbContext.AuditEvents.ToListAsync();
        savedEvents.Should().HaveCount(1);
        savedEvents.First().CorrelationId.Should().Be("corr-123");
    }

    [Fact]
    public async Task GetEvents_ShouldReturnLatestEvents()
    {
        // Arrange
        using var dbContext = new AuditDbContext(_dbContextOptions);
        var loggerMock = new Mock<ILogger<AuditEventsController>>();
        
        dbContext.AuditEvents.Add(new Domain.Models.AuditEvent(
            Guid.NewGuid(), "corr-1", "Test", "Test", "Test", "1", null, "Sys", "Sys", "Act", "Sum", "{}", DateTimeOffset.UtcNow, "Src", "Info"));
        await dbContext.SaveChangesAsync();

        var controller = new AuditEventsController(dbContext, loggerMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = await controller.GetEvents(CancellationToken.None);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        var response = okResult!.Value as ApiResponse<System.Collections.Generic.IEnumerable<Domain.Models.AuditEvent>>;
        response.Should().NotBeNull();
        response!.Data.Should().HaveCount(1);
    }
}
