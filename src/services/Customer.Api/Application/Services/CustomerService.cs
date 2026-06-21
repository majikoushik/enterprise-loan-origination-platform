using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Customer.Api.Application.DTOs;
using Customer.Api.Domain.Models;
using Customer.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Observability;
using Auditing;

namespace Customer.Api.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly CustomerDbContext _dbContext;
    private readonly ILogger<CustomerService> _logger;
    private readonly CorrelationIdProvider _correlationIdProvider;
    private readonly IAuditLogger _auditLogger;

    public CustomerService(CustomerDbContext dbContext, ILogger<CustomerService> logger, CorrelationIdProvider correlationIdProvider, IAuditLogger auditLogger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
        _correlationIdProvider = correlationIdProvider;
        _auditLogger = auditLogger;
    }

    public async Task<CustomerResponse> RegisterCustomerAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default)
    {
        // Domain validation happens in the entity constructor
        var customer = new CustomerProfile(
            request.FullName,
            request.Email,
            request.MobileNumber,
            request.DateOfBirth,
            request.EmploymentType,
            request.MonthlyIncome,
            request.ExistingMonthlyObligations
        );

        // Check if email already exists
        bool emailExists = await _dbContext.Customers
            .AnyAsync(c => c.Email == request.Email, cancellationToken);
            
        if (emailExists)
        {
            throw new CustomerDomainException("A customer with this email already exists.");
        }

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Customer registered successfully with Id: {CustomerId}", customer.Id);

        await _auditLogger.LogAsync(new AuditEventRecord(
            Guid.NewGuid(),
            _correlationIdProvider.Get(),
            "CustomerRegistered",
            "Customer",
            "Customer",
            customer.Id.ToString(),
            customer.Id,
            "Customer",
            "Self",
            "Register",
            $"Customer registered: {request.FullName}",
            System.Text.Json.JsonSerializer.Serialize(new { email = customer.Email.Value }),
            DateTimeOffset.UtcNow,
            "Customer.Api",
            "Info"
        ), cancellationToken);

        return MapToResponse(customer);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (customer == null)
            return null;

        return MapToResponse(customer);
    }

    public async Task<IEnumerable<CustomerResponse>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _dbContext.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        return customers.Select(MapToResponse);
    }

    private static CustomerResponse MapToResponse(CustomerProfile customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FullName,
            customer.Email,
            customer.MobileNumber,
            customer.DateOfBirth,
            customer.EmploymentType,
            customer.MonthlyIncome,
            customer.ExistingMonthlyObligations,
            customer.CreatedAt
        );
    }
}
