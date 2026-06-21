using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using LoanApplication.Api.Application.DTOs;
using LoanApplication.Api.Application.Services;
using LoanApplication.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.Api.Controllers;

[ApiController]
[Route("api/v1/loan-applications")]
public class LoanApplicationsController : ControllerBase
{
    private readonly ILoanApplicationService _loanApplicationService;
    private readonly IValidator<LoanApplicationRequest> _validator;

    public LoanApplicationsController(
        ILoanApplicationService loanApplicationService,
        IValidator<LoanApplicationRequest> validator)
    {
        _loanApplicationService = loanApplicationService;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoanApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitApplication([FromBody] LoanApplicationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var problemDetails = new ValidationProblemDetails(validationResult.ToDictionary())
            {
                Type = "https://example.com/problems/validation-error",
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred."
            };
            return BadRequest(problemDetails);
        }

        try
        {
            var response = await _loanApplicationService.SubmitApplicationAsync(request);
            return CreatedAtAction(nameof(GetApplication), new { id = response.Id }, response);
        }
        catch (LoanApplicationDomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://example.com/problems/domain-rule-violation",
                Title = "Domain Rule Violation",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LoanApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        var response = await _loanApplicationService.GetApplicationByIdAsync(id);
        if (response == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://example.com/problems/not-found",
                Title = "Application Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Loan application with ID {id} was not found."
            });
        }

        return Ok(response);
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(IEnumerable<LoanApplicationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicationsByCustomerId(Guid customerId)
    {
        var response = await _loanApplicationService.GetApplicationsByCustomerIdAsync(customerId);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LoanApplicationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllApplications()
    {
        var response = await _loanApplicationService.GetAllApplicationsAsync();
        return Ok(response);
    }
}
