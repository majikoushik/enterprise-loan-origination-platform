using Auditing;
using Audit.Api.Domain.Models;
using Audit.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Audit.Api.Controllers;

[ApiController]
[Route("api/v1/audit")]
public class AuditEventsController : ControllerBase
{
    private readonly AuditDbContext _dbContext;
    private readonly ILogger<AuditEventsController> _logger;

    public AuditEventsController(AuditDbContext dbContext, ILogger<AuditEventsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("events")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IngestEvent([FromBody] AuditEventRecord record, CancellationToken cancellationToken)
    {
        var auditEvent = new AuditEvent(
            record.EventId,
            record.CorrelationId,
            record.EventType,
            record.Category,
            record.EntityType,
            record.EntityId,
            record.CustomerId,
            record.ActorType,
            record.ActorId,
            record.Action,
            record.Summary,
            record.MetadataJson,
            record.OccurredAtUtc,
            record.SourceService,
            record.Severity
        );

        _dbContext.AuditEvents.Add(auditEvent);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Ingested audit event {EventId} of type {EventType} from {SourceService}", record.EventId, record.EventType, record.SourceService);

        return Created($"/api/v1/audit/events/{record.EventId}", null);
    }

    [HttpGet("events")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditEvent>>>> GetEvents(CancellationToken cancellationToken)
    {
        var events = await _dbContext.AuditEvents
            .OrderByDescending(e => e.OccurredAtUtc)
            .Take(100)
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<AuditEvent>>.Create(events, GetCorrelationId()));
    }

    [HttpGet("entity/{entityType}/{entityId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditEvent>>>> GetEventsByEntity(string entityType, string entityId, CancellationToken cancellationToken)
    {
        var events = await _dbContext.AuditEvents
            .Where(e => e.EntityType == entityType && e.EntityId == entityId)
            .OrderByDescending(e => e.OccurredAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<AuditEvent>>.Create(events, GetCorrelationId()));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditEvent>>>> GetEventsByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var events = await _dbContext.AuditEvents
            .Where(e => e.CustomerId == customerId)
            .OrderByDescending(e => e.OccurredAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<AuditEvent>>.Create(events, GetCorrelationId()));
    }

    [HttpGet("correlation/{correlationId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditEvent>>>> GetEventsByCorrelation(string correlationId, CancellationToken cancellationToken)
    {
        var events = await _dbContext.AuditEvents
            .Where(e => e.CorrelationId == correlationId)
            .OrderByDescending(e => e.OccurredAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        return Ok(ApiResponse<IEnumerable<AuditEvent>>.Create(events, GetCorrelationId()));
    }

    private string GetCorrelationId()
    {
        return HttpContext.Items["X-Correlation-ID"]?.ToString() ?? string.Empty;
    }
}
