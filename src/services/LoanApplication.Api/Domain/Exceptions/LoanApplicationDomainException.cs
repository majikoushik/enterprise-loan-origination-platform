using System;

namespace LoanApplication.Api.Domain.Exceptions;

public class LoanApplicationDomainException : Exception
{
    public LoanApplicationDomainException()
    { }

    public LoanApplicationDomainException(string message)
        : base(message)
    { }

    public LoanApplicationDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
