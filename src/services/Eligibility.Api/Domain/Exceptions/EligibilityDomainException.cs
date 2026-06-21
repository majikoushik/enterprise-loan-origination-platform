using SharedKernel.Exceptions;

namespace Eligibility.Api.Domain.Exceptions;

public class EligibilityDomainException : DomainException
{
    public EligibilityDomainException(string message)
        : base(message)
    { }
}
