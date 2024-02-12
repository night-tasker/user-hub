using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Jobs;
using NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Settings;
using Quartz;

namespace NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Configuration;

public static class OutboxMessagesConfiguration
{
    public static OutboxMessagesSettings AddOutboxMessagesSettings(
        this IServiceCollection services, IConfiguration configuration)
    {
        var outboxMessagesSettingsSection = configuration.GetSection(nameof(OutboxMessagesSettings));
        var outboxMessagesSettings = new OutboxMessagesSettings();
        outboxMessagesSettingsSection.Bind(outboxMessagesSettings);
        services.Configure<OutboxMessagesSettings>(outboxMessagesSettingsSection);
        return outboxMessagesSettings;
    }
    
    public static void AddOutboxMessagesJobs(
        this IServiceCollectionQuartzConfigurator quartzConfigurator,
        OutboxMessagesSettings outboxMessagesSettings)
    {
        var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
        quartzConfigurator.AddJob<ProcessOutboxMessagesJob>(jobKey);
        quartzConfigurator.AddTrigger(triggerConfig =>
        {
            triggerConfig.ForJob(jobKey)
                .WithCronSchedule(outboxMessagesSettings.CronExpression);
        });
    }
}