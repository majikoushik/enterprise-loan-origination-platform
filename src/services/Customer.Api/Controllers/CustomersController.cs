using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer.Api.Application.DTOs;
using Customer.Api.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CustomerRegistrationRequest> _validator;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CustomerRegistrationRequest> validator)
    {
        _customerService = customerService;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationRequest request)
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
            var response = await _customerService.RegisterCustomerAsync(request);
            return CreatedAtAction(nameof(GetCustomer), new { id = response.Id }, response);
        }
        catch (Domain.CustomerDomainException ex)
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
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);
        if (response == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://example.com/problems/not-found",
                Title = "Customer Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Customer with ID {id} was not found."
            });
        }

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers()
    {
        var response = await _customerService.GetAllCustomersAsync();
        return Ok(response);
    }
}
