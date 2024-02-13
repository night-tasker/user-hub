using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NightTasker.UserHub.Core.Domain.Primitives;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.Infrastructure.Persistence.Outbox;
using NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Settings;
using Quartz;

namespace NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Jobs;

public class ProcessOutboxMessagesJob(
    ApplicationDbContext applicationDbContext,
    IOptions<OutboxMessagesSettings> outboxMessagesSettings,
    IPublisher publisher,
    ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private readonly ApplicationDbContext _applicationDbContext = 
        applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    private readonly OutboxMessagesSettings _outboxMessagesSettings =
        outboxMessagesSettings.Value ?? throw new ArgumentNullException(nameof(outboxMessagesSettings));
    private readonly IPublisher _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = 
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await GetOutboxMessages(context.CancellationToken);
        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                await ProcessOutboxMessage(outboxMessage, context.CancellationToken);
                outboxMessage.MarkAsProcessed();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to process outbox message {OutboxMessageId}", outboxMessage.Id);
                outboxMessage.MarkAsFailed(JsonConvert.SerializeObject(exception));
            }
            _applicationDbContext.Update(outboxMessage);
        }
        await _applicationDbContext.SaveChangesAsync(context.CancellationToken);
    }
    
    private async Task ProcessOutboxMessage(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        
        if (domainEvent is null)
        {
            return;
        }
        
        await _publisher.Publish(domainEvent, cancellationToken);
    }

    private async Task<IReadOnlyCollection<OutboxMessage>> GetOutboxMessages(CancellationToken cancellationToken)
    {
        var messagesCount = _outboxMessagesSettings.CountOfOutboxMessagesToProcessPerJob;
        var messages = await _applicationDbContext
            .Set<OutboxMessage>()
            .Where(x => !x.IsProcessed)
            .Take(messagesCount)
            .ToListAsync(cancellationToken);
        return messages;
    }
}