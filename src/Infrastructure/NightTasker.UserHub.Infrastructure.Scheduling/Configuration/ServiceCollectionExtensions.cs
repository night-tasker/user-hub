using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Scheduling.BackgroundJobs.Outbox.Configuration;
using Quartz;
using Quartz.Simpl;

namespace NightTasker.UserHub.Infrastructure.Scheduling.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSchedulingServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var outboxMessagesSettings = services.AddOutboxMessagesSettings(configuration);
        
        services.AddQuartz(quartzConfig =>
        {
            quartzConfig.AddOutboxMessagesJobs(outboxMessagesSettings);
            quartzConfig.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService();
        
        return services;
    }
}