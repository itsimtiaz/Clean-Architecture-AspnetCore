namespace Domain.DomainExceptions;

public class AgeNotAllowedException(int value) : DomainBaseException($"Age {value} is not allowed.")
{

}
