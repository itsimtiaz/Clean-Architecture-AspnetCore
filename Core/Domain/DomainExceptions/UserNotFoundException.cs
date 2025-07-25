namespace Domain.DomainExceptions;

public class UserNotFoundException(int id) : DomainBaseException($"User with id {id} not found.")
{

}
