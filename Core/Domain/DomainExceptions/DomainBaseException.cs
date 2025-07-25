namespace Domain.DomainExceptions;

public class DomainBaseException : Exception
{
    public DomainBaseException(string? message) : base(message)
    {

    }
}
