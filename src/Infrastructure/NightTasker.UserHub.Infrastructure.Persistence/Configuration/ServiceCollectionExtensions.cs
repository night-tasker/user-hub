using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Infrastructure.Persistence.Repository.Common;

namespace NightTasker.UserHub.Infrastructure.Persistence.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Database"));
        });
        return services;
    }
}