using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer.Api.Application.DTOs;
using Customer.Api.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Observability;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedValidationException = SharedKernel.Exceptions.ValidationException;

namespace Customer.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CustomerRegistrationRequest> _validator;
    private readonly CorrelationIdProvider _correlationIdProvider;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CustomerRegistrationRequest> validator,
        CorrelationIdProvider correlationIdProvider)
    {
        _customerService = customerService;
        _validator = validator;
        _correlationIdProvider = correlationIdProvider;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new SharedValidationException(validationResult.ToDictionary());
        }

        var response = await _customerService.RegisterCustomerAsync(request);
        return CreatedAtAction(nameof(GetCustomer), new { id = response.Id }, ApiResponse<CustomerResponse>.Create(response, GetCorrelationId()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);
        if (response == null)
        {
            throw new NotFoundException($"Customer with ID {id} was not found.");
        }

        return Ok(ApiResponse<CustomerResponse>.Create(response, GetCorrelationId()));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers()
    {
        var response = await _customerService.GetAllCustomersAsync();
        return Ok(ApiResponse<IEnumerable<CustomerResponse>>.Create(response, GetCorrelationId()));
    }

    private string GetCorrelationId() => _correlationIdProvider.Get();
}
