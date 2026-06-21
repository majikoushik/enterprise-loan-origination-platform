using SharedKernel.Exceptions;

namespace LoanApplication.Api.Domain.Exceptions;

public class LoanApplicationDomainException : DomainException
{
    public LoanApplicationDomainException(string message)
        : base(message)
    { }
}
