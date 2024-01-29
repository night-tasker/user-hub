using System.Data.Common;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;

namespace NightTasker.UserHub.Presentation.WebApi.IntegrationTests;

public class TestWebApplicationFactory(
    TestNpgSql testDatabase,
    IReadOnlyCollection<ServiceForRegister>? mockedServices = null,
    bool mockAuthorization = false) : WebApplicationFactory<Program>
{
    private readonly string _connectionString = testDatabase.NpgSqlContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceDbContextService(services);

            if (mockAuthorization)
            {
                MockAuthorization(services);
            }
            
            if (mockedServices is not null)
            {
                MockServices(services, mockedServices);
            }
        });
    }

    private void ReplaceDbContextService(IServiceCollection serviceCollection)
    {
        serviceCollection.Remove(serviceCollection.SingleOrDefault(service =>
            typeof(DbContextOptions<ApplicationDbContext>) == service.ServiceType)!);
        serviceCollection.Remove(serviceCollection.SingleOrDefault(service =>
            typeof(DbConnection) == service.ServiceType)!);
        serviceCollection.AddDbContext<ApplicationDbContext>((_, option) => option.UseNpgsql(_connectionString));

    }

    private void MockServices(
        IServiceCollection serviceCollection, IReadOnlyCollection<ServiceForRegister> mockedServicesCollection)
    {
        foreach (var mockedService in mockedServicesCollection)
        {
            var serviceToDelete = serviceCollection.SingleOrDefault(service => service.ServiceType == mockedService.Type);
            serviceCollection.Remove(serviceToDelete!);
            AddServiceDependingOnLifeTime(serviceCollection, mockedService);
        }
    }
    
    private void MockAuthorization(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
    }

    private void AddServiceDependingOnLifeTime(IServiceCollection services, ServiceForRegister serviceForRegister)
    {
        if (serviceForRegister.Lifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton(serviceForRegister.Type, serviceForRegister.Factory);
        }

        if (serviceForRegister.Lifetime == ServiceLifetime.Scoped)
        {
            services.AddScoped(serviceForRegister.Type, serviceForRegister.Factory);
        }

        if (serviceForRegister.Lifetime == ServiceLifetime.Transient)
        {
            services.AddTransient(serviceForRegister.Type, serviceForRegister.Factory);
        }
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        return host;
    }
}