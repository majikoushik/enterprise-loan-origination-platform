using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Observability;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedValidationException = SharedKernel.Exceptions.ValidationException;

namespace LoanApplication.Api.Controllers;

[ApiController]
[Route("api/v1/loan-applications")]
public class LoanApplicationsController : ControllerBase
{
    private readonly ILoanApplicationService _loanApplicationService;
    private readonly IValidator<LoanApplicationRequest> _validator;
    private readonly CorrelationIdProvider _correlationIdProvider;

    public LoanApplicationsController(
        ILoanApplicationService loanApplicationService,
        IValidator<LoanApplicationRequest> validator,
        CorrelationIdProvider correlationIdProvider)
    {
        _loanApplicationService = loanApplicationService;
        _validator = validator;
        _correlationIdProvider = correlationIdProvider;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LoanApplicationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitApplication([FromBody] LoanApplicationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new SharedValidationException(validationResult.ToDictionary());
        }

        var response = await _loanApplicationService.SubmitApplicationAsync(request);
        return CreatedAtAction(nameof(GetApplication), new { id = response.Id }, ApiResponse<LoanApplicationResponse>.Create(response, GetCorrelationId()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LoanApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        var response = await _loanApplicationService.GetApplicationByIdAsync(id);
        if (response == null)
        {
            throw new NotFoundException($"Loan application with ID {id} was not found.");
        }

        return Ok(ApiResponse<LoanApplicationResponse>.Create(response, GetCorrelationId()));
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoanApplicationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicationsByCustomerId(Guid customerId)
    {
        var response = await _loanApplicationService.GetApplicationsByCustomerIdAsync(customerId);
        return Ok(ApiResponse<IEnumerable<LoanApplicationResponse>>.Create(response, GetCorrelationId()));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoanApplicationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllApplications()
    {
        var result = await _loanApplicationService.GetAllApplicationsAsync();
        return Ok(ApiResponse<IEnumerable<LoanApplicationResponse>>.Create(result, GetCorrelationId()));
    }

    [HttpGet("{id}/status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplicationStatus(Guid id)
    {
        var application = await _loanApplicationService.GetApplicationByIdAsync(id);
        if (application == null)
        {
            throw new NotFoundException($"No loan application found with ID {id}.");
        }

        return Ok(ApiResponse<object>.Create(new { status = application.Status.ToString() }, GetCorrelationId()));
    }

    [HttpGet("{id}/status-history")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ApplicationStatusHistoryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplicationStatusHistory(Guid id)
    {
        var history = await _loanApplicationService.GetStatusHistoryAsync(id);
        return Ok(ApiResponse<IEnumerable<ApplicationStatusHistoryResponse>>.Create(history, GetCorrelationId()));
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(ApiResponse<LoanApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateApplicationStatus(Guid id, [FromBody] UpdateApplicationStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _loanApplicationService.UpdateStatusAsync(id, request);
        return Ok(ApiResponse<LoanApplicationResponse>.Create(result, GetCorrelationId()));
    }

    private string GetCorrelationId() => _correlationIdProvider.Get();
}
