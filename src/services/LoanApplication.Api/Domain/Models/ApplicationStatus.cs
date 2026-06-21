namespace LoanApplication.Api.Domain.Models;

public enum ApplicationStatus
{
    Draft = 1,
    Submitted = 2,
    UnderReview = 3,
    EligibilityPassed = 4,
    EligibilityFailed = 5,
    Approved = 6,
    Rejected = 7,
    Cancelled = 8
}
