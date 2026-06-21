using System;
using System.Threading.Tasks;
using Eligibility.Api.Application.DTOs;
using Eligibility.Api.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Observability;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedValidationException = SharedKernel.Exceptions.ValidationException;

namespace Eligibility.Api.Controllers;

[ApiController]
[Route("api/v1/eligibility")]
public class EligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;
    private readonly IValidator<EvaluateEligibilityRequest> _validator;
    private readonly CorrelationIdProvider _correlationIdProvider;

    public EligibilityController(
        IEligibilityService eligibilityService,
        IValidator<EvaluateEligibilityRequest> validator,
        CorrelationIdProvider correlationIdProvider)
    {
        _eligibilityService = eligibilityService;
        _validator = validator;
        _correlationIdProvider = correlationIdProvider;
    }

    [HttpPost("check")]
    [ProducesResponseType(typeof(ApiResponse<EligibilityResultResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EvaluateEligibility([FromBody] EvaluateEligibilityRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new SharedValidationException(validationResult.ToDictionary());
        }

        var result = await _eligibilityService.EvaluateAsync(request);
        return CreatedAtAction(nameof(GetEligibilityResultById), new { id = result.Id }, ApiResponse<EligibilityResultResponse>.Create(result, GetCorrelationId()));
    }

    [HttpGet("applications/{applicationId}")]
    [ProducesResponseType(typeof(ApiResponse<EligibilityResultResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEligibilityResultByApplicationId(Guid applicationId)
    {
        var result = await _eligibilityService.GetByApplicationIdAsync(applicationId);
        if (result == null)
        {
            throw new NotFoundException($"No eligibility result found for Application ID {applicationId}.");
        }

        return Ok(ApiResponse<EligibilityResultResponse>.Create(result, GetCorrelationId()));
    }

    [HttpGet("results/{id}")]
    [ProducesResponseType(typeof(ApiResponse<EligibilityResultResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEligibilityResultById(Guid id)
    {
        var result = await _eligibilityService.GetByIdAsync(id);
        if (result == null)
        {
            throw new NotFoundException($"No eligibility result found with ID {id}.");
        }

        return Ok(ApiResponse<EligibilityResultResponse>.Create(result, GetCorrelationId()));
    }

    private string GetCorrelationId() => _correlationIdProvider.Get();
}
