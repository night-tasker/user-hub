namespace NightTasker.UserHub.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage(
        Guid id,
        string type,
        string content)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOn = DateTimeOffset.Now;
    }
    
    public static OutboxMessage CreateInstance(
        string type,
        string content)
    {
        return new OutboxMessage(Guid.NewGuid(), type, content);
    }
    
    public Guid Id { get; private set; }

    public string Type { get; private set; }
    
    public string Content { get; set; }
    
    public DateTimeOffset OccurredOn { get; private set; }
    
    public DateTimeOffset? ProcessedOn { get; private set; }

    public bool IsProcessed { get; private set; }
    
    public string? Error { get; private set; }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
        ProcessedOn = DateTimeOffset.Now;
    }
}