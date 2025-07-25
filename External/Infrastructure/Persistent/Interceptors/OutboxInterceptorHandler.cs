using System.Text.Json;
using System.Threading.Channels;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistent.Events;

namespace Persistent.Interceptors;

internal class OutboxInterceptorHandler : SaveChangesInterceptor
{
    private readonly Channel<BackgroundTriggerEvent> _updateChannel;

    public OutboxInterceptorHandler(Channel<BackgroundTriggerEvent> updateChannel)
    {
        _updateChannel = updateChannel;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var aggregates = context.ChangeTracker.Entries<AggregateRoot>().Where(_ => _.Entity.DomainEvents.Any()).Select(_ => _.Entity);

        var domainEvents = aggregates.SelectMany(e => e.DomainEvents.ToList());

        List<OutBoxMessage> outBoxMessages = new();

        foreach (var domainEvent in domainEvents)
        {
            OutBoxMessage outBoxMessage = new()
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(domainEvent),
                DateTimeOccurredOn = DateTime.UtcNow
            };

            outBoxMessages.Add(outBoxMessage);
        }

        context.Set<OutBoxMessage>().AddRange(outBoxMessages);

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearEvents();
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        bool domainAdded = context.ChangeTracker.Entries<OutBoxMessage>().Select(_ => _.Entity).Any(_ => _.DateTimeProcessedOn == null);

        var eventResult = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (domainAdded && await _updateChannel.Writer.WaitToWriteAsync(cancellationToken))
        {
            await _updateChannel.Writer.WriteAsync(new BackgroundTriggerEvent(), cancellationToken);
        }

        return eventResult;
    }

}