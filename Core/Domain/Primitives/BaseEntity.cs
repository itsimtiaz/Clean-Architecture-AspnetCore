namespace Domain.Primitives;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public uint Id { get; private init; }

    protected BaseEntity(uint id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        if (obj is BaseEntity entity)
            return Id == entity.Id;

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(BaseEntity? firstEntity, BaseEntity? secondEntity)
    {
        return firstEntity is not null && secondEntity is not null && firstEntity.Equals(secondEntity);
    }

    public static bool operator !=(BaseEntity? firstEntity, BaseEntity secondEntity)
    {
        return !(firstEntity == secondEntity);
    }
}
