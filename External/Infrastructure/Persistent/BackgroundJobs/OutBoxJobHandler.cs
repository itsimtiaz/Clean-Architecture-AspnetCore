using System.Text.Json;
using System.Threading.Channels;
using Domain.Interfaces;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistent.Data;
using Persistent.Events;

namespace Persistent.BackgroundJobs;

internal class OutBoxJobHandler : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly Channel<BackgroundTriggerEvent> channel;

    public OutBoxJobHandler(IServiceProvider serviceProvider, Channel<BackgroundTriggerEvent> channel)
    {
        this.serviceProvider = serviceProvider;
        this.channel = channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            // Just to flash out the channel
            await channel.Reader.ReadAsync(stoppingToken);

            using var scope = serviceProvider.CreateAsyncScope();

            var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            var outboxMessages = await writeDbContext.OutBoxMessages
            .Where(_ => _.DateTimeProcessedOn == null).Take(10)
            .ToListAsync(stoppingToken);

            foreach (var message in outboxMessages)
            {
                Type? domainEventType = Type.GetType(message.Type);

                if (domainEventType is null)
                    continue;

                var domainEvent = JsonSerializer.Deserialize(message.Content, domainEventType) as IDomainEvent;

                if (domainEvent is null)
                    continue;

                await publisher.Publish(domainEvent);

                message.DateTimeProcessedOn = DateTime.UtcNow;
            }

            if (outboxMessages.Count > 0)
            {
                await writeDbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
