using System;
using System.Threading.Tasks;
using Eligibility.Api.Application.DTOs;
using Eligibility.Api.Application.Services;
using Eligibility.Api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eligibility.Api.Controllers;

[ApiController]
[Route("api/v1/eligibility")]
public class EligibilityController : ControllerBase
{
    private readonly IEligibilityService _eligibilityService;

    public EligibilityController(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    [HttpPost("check")]
    [ProducesResponseType(typeof(EligibilityResultResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EvaluateEligibility([FromBody] EvaluateEligibilityRequest request)
    {
        if (request.ApplicationId == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://example.com/problems/validation-error",
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = "ApplicationId is required."
            });
        }

        try
        {
            var result = await _eligibilityService.EvaluateAsync(request);
            // Ideally we could point to GetEligibilityResultById, but we can also just return OK
            return CreatedAtAction(nameof(GetEligibilityResultById), new { id = result.Id }, result);
        }
        catch (EligibilityDomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://example.com/problems/domain-rule-violation",
                Title = "Domain Exception",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });
        }
    }

    [HttpGet("applications/{applicationId}")]
    [ProducesResponseType(typeof(EligibilityResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEligibilityResultByApplicationId(Guid applicationId)
    {
        var result = await _eligibilityService.GetByApplicationIdAsync(applicationId);
        if (result == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://example.com/problems/not-found",
                Title = "Eligibility Result Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No eligibility result found for Application ID {applicationId}."
            });
        }

        return Ok(result);
    }

    [HttpGet("results/{id}")]
    [ProducesResponseType(typeof(EligibilityResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEligibilityResultById(Guid id)
    {
        var result = await _eligibilityService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://example.com/problems/not-found",
                Title = "Eligibility Result Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No eligibility result found with ID {id}."
            });
        }

        return Ok(result);
    }
}
