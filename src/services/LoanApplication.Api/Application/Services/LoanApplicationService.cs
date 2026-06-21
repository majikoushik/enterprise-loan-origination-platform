using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Domain.Exceptions;
using LoanApplication.Api.Domain.Models;
using LoanApplication.Api.Infrastructure.Data;
using Messaging;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.Api.Application.Services;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly LoanApplicationDbContext _dbContext;
    private readonly ICustomerLookupService _customerLookupService;
    private readonly IMessagePublisher _messagePublisher;

    public LoanApplicationService(
        LoanApplicationDbContext dbContext,
        ICustomerLookupService customerLookupService,
        IMessagePublisher messagePublisher)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _customerLookupService = customerLookupService ?? throw new ArgumentNullException(nameof(customerLookupService));
        _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
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

        // TODO: In a future epic, publish a Domain/Integration Event (e.g. LoanApplicationSubmitted)
        // for Audit and Notification services.

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
            .Include("_statusHistory")
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (application == null)
        {
            throw new LoanApplicationDomainException($"Loan application with ID {id} could not be found.");
        }

        if (!Enum.TryParse<ApplicationStatus>(request.NewStatus, true, out var newStatusEnum))
        {
            throw new LoanApplicationDomainException($"Invalid application status: {request.NewStatus}");
        }

        application.ChangeStatus(newStatusEnum, request.Reason, request.ChangedBy);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Publish event
        var integrationEvent = new LoanApplicationStatusChangedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            Guid.NewGuid().ToString(), // simple correlation id
            application.Id,
            application.StatusHistory.Last().PreviousStatus?.ToString() ?? "Draft",
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
            .Include("_statusHistory")
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
