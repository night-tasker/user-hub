using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NightTasker.UserHub.Infrastructure.Persistence.Configuration;

/// <summary>
/// Класс для методов расширения для коллекций сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать сервисы для <see cref="NightTasker.UserHub.Infrastructure.Persistence"/> слоя.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <returns>Коллекция сервисов.</returns>
    
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Database"));
        });
        return services;
    }
}