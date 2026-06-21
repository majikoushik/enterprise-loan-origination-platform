using System.ComponentModel.DataAnnotations;

namespace LoanApplication.Api.Application.DTOs;

public class UpdateApplicationStatusRequest
{
    [Required]
    public string NewStatus { get; set; } = string.Empty;

    [Required]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public string ChangedBy { get; set; } = string.Empty;
}
