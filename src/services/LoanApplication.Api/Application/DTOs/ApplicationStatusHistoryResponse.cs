using System;

namespace LoanApplication.Api.Application.DTOs;

public record ApplicationStatusHistoryResponse(
    Guid Id,
    Guid ApplicationId,
    string? PreviousStatus,
    string NewStatus,
    string Reason,
    string ChangedBy,
    DateTime ChangedAt
);
