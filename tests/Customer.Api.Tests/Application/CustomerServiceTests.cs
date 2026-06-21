using System;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using Customer.Api.Application.DTOs;
using Customer.Api.Application.Services;
using Customer.Api.Domain;
using Customer.Api.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Observability;
using Xunit;

namespace Customer.Api.Tests.Application;

public class CustomerServiceTests
{
    private static CustomerService CreateService(CustomerDbContext dbContext)
    {
        var auditLoggerMock = new Mock<IAuditLogger>();
        auditLoggerMock
            .Setup(a => a.LogAsync(It.IsAny<AuditEventRecord>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new CustomerService(
            dbContext,
            new Mock<ILogger<CustomerService>>().Object,
            new CorrelationIdProvider(new Mock<IHttpContextAccessor>().Object),
            auditLoggerMock.Object);
    }

    private DbContextOptions<CustomerDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task RegisterCustomerAsync_Should_Create_Customer()
    {
        // Arrange
        var dbContext = new CustomerDbContext(GetDbContextOptions());
        var service = CreateService(dbContext);

        var request = new CustomerRegistrationRequest(
            "Jane Doe",
            "jane@example.com",
            "+1987654321",
            new DateTime(1985, 5, 15),
            EmploymentType.SelfEmployed,
            8000,
            2000
        );

        // Act
        var response = await service.RegisterCustomerAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.FullName.Should().Be(request.FullName);
        
        var dbCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
        dbCustomer.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterCustomerAsync_Should_Throw_When_Email_Exists()
    {
        // Arrange
        var dbContext = new CustomerDbContext(GetDbContextOptions());
        var service = CreateService(dbContext);

        var request = new CustomerRegistrationRequest(
            "Jane Doe",
            "jane@example.com",
            "+1987654321",
            new DateTime(1985, 5, 15),
            EmploymentType.SelfEmployed,
            8000,
            2000
        );

        await service.RegisterCustomerAsync(request, CancellationToken.None);

        // Act & Assert
        await FluentActions.Invoking(() => service.RegisterCustomerAsync(request, CancellationToken.None))
            .Should().ThrowAsync<CustomerDomainException>()
            .WithMessage("*email already exists*");
    }
}
