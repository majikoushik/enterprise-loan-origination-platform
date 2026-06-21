using System;

namespace Eligibility.Api.Domain.Exceptions;

public class EligibilityDomainException : Exception
{
    public EligibilityDomainException()
    { }

    public EligibilityDomainException(string message)
        : base(message)
    { }

    public EligibilityDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
