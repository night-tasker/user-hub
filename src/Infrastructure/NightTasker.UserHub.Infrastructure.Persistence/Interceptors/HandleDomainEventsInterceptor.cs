using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using NightTasker.UserHub.Core.Domain.Primitives;
using NightTasker.UserHub.Infrastructure.Persistence.Outbox;

namespace NightTasker.UserHub.Infrastructure.Persistence.Interceptors;

public class HandleDomainEventsInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        var entries = GetAggregateRootEntries(dbContext);
        var events = CollectDomainEventsFromAggregateRoots(entries);
        var outboxMessages = ConvertDomainEventsToOutboxMessages(events);
        if (outboxMessages.Any())
        {
            await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static IEnumerable<IAggregateRoot> GetAggregateRootEntries(DbContext dbContext)
    {
        return dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity);
    }
    
    private static IEnumerable<IDomainEvent> CollectDomainEventsFromAggregateRoots(IEnumerable<IAggregateRoot> aggregateRoots)
    {
        return aggregateRoots
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            });
    }

    private static IReadOnlyCollection<OutboxMessage> ConvertDomainEventsToOutboxMessages(
        IEnumerable<IDomainEvent> domainEvents)
    {
        return domainEvents
            .Select(domainEvent => OutboxMessage.CreateInstance(
                    domainEvent.GetType().Name, 
                    ConvertDomainEventContentToJsonString(domainEvent)))
            .ToList();
    }

    private static string ConvertDomainEventContentToJsonString(IDomainEvent domainEvent)
    {
        return JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
    }
}