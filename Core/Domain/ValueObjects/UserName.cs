using Domain.DomainExceptions;

namespace Domain.ValueObjects;

public class UserName
{
    public string Value { get; }

    public UserName(string value)
    {
        if (value.Length > 20)
            throw new UserNameTooLongException();

        Value = value;
    }

    public static implicit operator UserName(string value) => new(value);
    public static implicit operator string(UserName value) => value.Value;
}
