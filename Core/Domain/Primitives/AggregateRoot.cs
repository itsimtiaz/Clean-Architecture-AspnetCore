using Domain.Interfaces;

namespace Domain.Primitives;

public abstract class AggregateRoot : BaseEntity
{
    public AggregateRoot(uint id) : base(id)
    {
    }

    readonly List<IDomainEvent> events = new();

    protected void AddDomainEvent(IDomainEvent domainEvent) => events.Add(domainEvent);

    public void ClearEvents() => events.Clear();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => events;
}
