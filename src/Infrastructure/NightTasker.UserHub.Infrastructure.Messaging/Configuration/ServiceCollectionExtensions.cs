using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Identity.Infrastructure.Messaging.Settings;

namespace NightTasker.UserHub.Infrastructure.Messaging.Configuration;

/// <summary>
/// Класс методов расширений для коллекции сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация служб для слоя <see cref="NightTasker.Identity.Infrastructure.Messaging"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <returns></returns>
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