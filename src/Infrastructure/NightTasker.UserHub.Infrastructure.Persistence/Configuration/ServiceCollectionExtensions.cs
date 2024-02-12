using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Domain.Repositories;
using NightTasker.UserHub.Infrastructure.Persistence.Interceptors;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

namespace NightTasker.UserHub.Infrastructure.Persistence.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<HandleDomainEventsInterceptor>();
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options)=>
        {
            var domainEventsInterceptor = serviceProvider.GetRequiredService<HandleDomainEventsInterceptor>();
            options
                .UseNpgsql(configuration.GetConnectionString("Database"))
                .AddInterceptors(domainEventsInterceptor);
        });
        return services;
    }
}