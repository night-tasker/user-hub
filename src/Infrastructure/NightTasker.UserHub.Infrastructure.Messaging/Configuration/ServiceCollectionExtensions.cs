using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Messaging.Settings;

namespace NightTasker.UserHub.Infrastructure.Messaging.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMessagingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqSettingsSection = configuration.GetSection(nameof(RabbitMqSettings));
        services.Configure<RabbitMqSettings>(rabbitMqSettingsSection);
        var rabbitMqSettings = rabbitMqSettingsSection.Get<RabbitMqSettings>()!;
        
        services.AddMassTransit(massTransitConfig =>
        {
            massTransitConfig.AddConsumers(typeof(ServiceCollectionExtensions).Assembly);
            massTransitConfig.UsingRabbitMq((context, rabbitMqConfig) =>
            {
                rabbitMqConfig.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, hostConfig =>
                {
                    hostConfig.Username(rabbitMqSettings.Username);
                    hostConfig.Password(rabbitMqSettings.Password);
                });

                rabbitMqConfig.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}