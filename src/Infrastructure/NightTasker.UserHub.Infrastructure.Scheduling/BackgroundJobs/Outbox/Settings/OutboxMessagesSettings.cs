namespace NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Settings;

public class OutboxMessagesSettings
{
    public int CountOfOutboxMessagesToProcessPerJob { get; set; }

    public string CronExpression { get; set; } = null!;
}