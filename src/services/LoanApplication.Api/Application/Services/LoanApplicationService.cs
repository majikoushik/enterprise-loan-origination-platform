using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Domain.Exceptions;
using LoanApplication.Api.Domain.Models;
using LoanApplication.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.Api.Application.Services;

public class LoanApplicationService : ILoanApplicationService
{
    private readonly LoanApplicationDbContext _dbContext;
    private readonly ICustomerLookupService _customerLookupService;

    public LoanApplicationService(
        LoanApplicationDbContext dbContext,
        ICustomerLookupService customerLookupService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _customerLookupService = customerLookupService ?? throw new ArgumentNullException(nameof(customerLookupService));
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
