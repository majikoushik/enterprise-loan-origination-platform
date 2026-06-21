using SharedKernel.Exceptions;

namespace Customer.Api.Domain;

public class CustomerDomainException : DomainException
{
    public CustomerDomainException(string message)
        : base(message)
    { }
}
