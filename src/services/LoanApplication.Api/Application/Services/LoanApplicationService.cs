using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Domain.Exceptions;
using LoanApplication.Api.Domain.Models;
using LoanApplication.Api.Infrastructure.Data;
using Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Observability;

namespace LoanApplication.Api.Application.Services;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly LoanApplicationDbContext _dbContext;
    private readonly ICustomerLookupService _customerLookupService;
    private readonly ILogger<LoanApplicationService> _logger;
    private readonly CorrelationIdProvider _correlationIdProvider;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IAuditLogger _auditLogger;

    public LoanApplicationService(
        LoanApplicationDbContext dbContext,
        ICustomerLookupService customerLookupService,
        ILogger<LoanApplicationService> logger,
        CorrelationIdProvider correlationIdProvider,
        IMessagePublisher messagePublisher,
        IAuditLogger auditLogger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _customerLookupService = customerLookupService ?? throw new ArgumentNullException(nameof(customerLookupService));
        _logger = logger;
        _correlationIdProvider = correlationIdProvider;
        _messagePublisher = messagePublisher;
        _auditLogger = auditLogger;
    }

    public async Task<LoanApplicationResponse> SubmitApplicationAsync(LoanApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var customerExists = await _customerLookupService.CustomerExistsAsync(request.CustomerId, cancellationToken);
        if (!customerExists)
        {
            throw new LoanApplicationDomainException($"Customer with ID {request.CustomerId} does not exist.");
        }

        var entity = new LoanApplicationEntity(
            request.CustomerId,
            request.LoanType,
            request.RequestedAmount,
            request.RequestedTenureInMonths,
            request.Purpose,
            request.DeclaredMonthlyIncome,
            request.ExistingEmiObligations
        );

        _dbContext.LoanApplications.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loan application {ApplicationId} submitted for Customer {CustomerId}", entity.Id, entity.CustomerId);

        await _auditLogger.LogAsync(new AuditEventRecord(
            Guid.NewGuid(),
            _correlationIdProvider.Get(),
            "LoanApplicationSubmitted",
            "LoanApplication",
            "LoanApplication",
            entity.Id.ToString(),
            entity.CustomerId,
            "Customer",
            "Self",
            "Submit",
            $"Loan application submitted for {request.RequestedAmount}",
            System.Text.Json.JsonSerializer.Serialize(new { requestedAmount = entity.RequestedAmount, purpose = entity.Purpose }),
            DateTimeOffset.UtcNow,
            "LoanApplication.Api",
            "Info"
        ), cancellationToken);

        return MapToResponse(entity);
    }

    public async Task<LoanApplicationResponse?> GetApplicationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.LoanApplications
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<LoanApplicationResponse>> GetApplicationsByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var applications = await _dbContext.LoanApplications
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return applications.Select(MapToResponse);
    }

    public async Task<IEnumerable<LoanApplicationResponse>> GetAllApplicationsAsync(CancellationToken cancellationToken = default)
    {
        var applications = await _dbContext.LoanApplications
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

        return applications.Select(MapToResponse);
    }


    public async Task<LoanApplicationResponse> UpdateStatusAsync(Guid id, UpdateApplicationStatusRequest request, CancellationToken cancellationToken = default)
    {
        var application = await _dbContext.LoanApplications
            .Include(a => a.StatusHistory)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (application == null)
        {
            throw new LoanApplicationDomainException($"Loan application with ID {id} could not be found.");
        }

        if (!Enum.TryParse<ApplicationStatus>(request.NewStatus, true, out var newStatusEnum))
        {
            throw new LoanApplicationDomainException($"Invalid application status: {request.NewStatus}");
        }

        var oldStatus = application.Status;
        application.ChangeStatus(newStatusEnum, request.Reason, request.ChangedBy);
        var newHistoryRecord = application.StatusHistory.OrderByDescending(h => h.ChangedAt).First();
        if (oldStatus != application.Status)
        {
            _dbContext.ApplicationStatusHistories.Add(newHistoryRecord);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loan application {ApplicationId} status updated to {NewStatus}", application.Id, request.NewStatus);

        await _auditLogger.LogAsync(new AuditEventRecord(
            Guid.NewGuid(),
            _correlationIdProvider.Get(),
            "LoanApplicationStatusChanged",
            "StatusTransition",
            "LoanApplication",
            application.Id.ToString(),
            application.CustomerId,
            "System",
            "System",
            "ChangeStatus",
            $"Loan application status changed from {oldStatus} to {application.Status}",
            System.Text.Json.JsonSerializer.Serialize(new { oldStatus = oldStatus.ToString(), newStatus = application.Status.ToString() }),
            DateTimeOffset.UtcNow,
            "LoanApplication.Api",
            "Info"
        ), cancellationToken);

        // Publish event
        var integrationEvent = new LoanApplicationStatusChangedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            Guid.NewGuid().ToString(), // simple correlation id
            application.Id,
            newHistoryRecord.PreviousStatus?.ToString() ?? "Draft",
            newStatusEnum.ToString(),
            application.CustomerId
        );
        
        try
        {
            await _messagePublisher.PublishAsync(integrationEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            // For MVP, don't fail the transaction if notification simulation fails. 
            // In a production system, use Outbox Pattern.
            Console.WriteLine($"Warning: Failed to publish event. {ex.Message}");
        }

        return MapToResponse(application);
    }

    public async Task<IEnumerable<ApplicationStatusHistoryResponse>> GetStatusHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var application = await _dbContext.LoanApplications
            .Include(a => a.StatusHistory)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (application == null)
        {
            throw new LoanApplicationDomainException($"Loan application with ID {id} could not be found.");
        }

        return application.StatusHistory.OrderByDescending(h => h.ChangedAt).Select(h => new ApplicationStatusHistoryResponse(
            h.Id,
            h.ApplicationId,
            h.PreviousStatus?.ToString(),
            h.NewStatus.ToString(),
            h.Reason,
            h.ChangedBy,
            h.ChangedAt
        ));
    }

    private static LoanApplicationResponse MapToResponse(LoanApplicationEntity entity)
    {
        return new LoanApplicationResponse(
            entity.Id,
            entity.CustomerId,
            entity.LoanType,
            entity.RequestedAmount,
            entity.RequestedTenureInMonths,
            entity.Purpose,
            entity.DeclaredMonthlyIncome,
            entity.ExistingEmiObligations,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }
}
