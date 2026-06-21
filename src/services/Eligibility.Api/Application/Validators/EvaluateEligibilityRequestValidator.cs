using Eligibility.Api.Application.DTOs;
using FluentValidation;

namespace Eligibility.Api.Application.Validators;

public sealed class EvaluateEligibilityRequestValidator : AbstractValidator<EvaluateEligibilityRequest>
{
    public EvaluateEligibilityRequestValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("ApplicationId is required.");
    }
}
