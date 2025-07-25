using Domain.Events;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : AggregateRoot
{
    private User(uint id, UserName name, UserAge age) : base(id)
    {
        _name = name;
        _age = age;
    }

    private UserName _name;
    private UserAge _age;

    public static User Create(uint id, string name, int age)
    {
        User user = new(id, name, age);
        user.AddDomainEvent(new UserCreatedEvent());

        return user;
    }
}
