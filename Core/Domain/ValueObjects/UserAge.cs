using Domain.DomainExceptions;

namespace Domain.ValueObjects;

public class UserAge
{
    public UserAge(int value)
    {
        if (value < 18 || value > 50)
            throw new AgeNotAllowedException(value);

        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(UserAge value) => value.Value;
    public static implicit operator UserAge(int value) => new(value);
}
