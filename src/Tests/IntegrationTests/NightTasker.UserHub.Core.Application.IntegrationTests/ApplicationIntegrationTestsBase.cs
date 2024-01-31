using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Configuration;

namespace NightTasker.UserHub.Core.Application.IntegrationTests;

public abstract class ApplicationIntegrationTestsBase
{
    private readonly IServiceCollection _serviceCollection;
    private IServiceProvider _serviceProvider = null!;

    protected ApplicationIntegrationTestsBase()
    {
        lock (this)
        {
            var testNpgSql = new TestNpgSql();
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddDbContext<ApplicationDbContext>(
                (_, option) => option.UseNpgsql($"{testNpgSql.NpgSqlContainer.GetConnectionString()};Include Error Detail=true"));
            _serviceCollection.AddMediatR(
                conf => conf.RegisterServicesFromAssembly(typeof(GetOrganizationByIdAsUserQuery).Assembly));
            _serviceCollection.AddMapper();
        }
    }

    protected void RegisterService(ServiceForRegister serviceForRegister)
    {
        switch (serviceForRegister)
        {
            case { Lifetime: ServiceLifetime.Singleton }:
                _serviceCollection.AddSingleton(serviceForRegister.Type, serviceForRegister.Factory);
                break;
            case { Lifetime: ServiceLifetime.Scoped }:
                _serviceCollection.AddScoped(serviceForRegister.Type, serviceForRegister.Factory);
                break;
            case { Lifetime: ServiceLifetime.Transient }:
                _serviceCollection.AddTransient(serviceForRegister.Type, serviceForRegister.Factory);
                break;
        }
    }

    protected void BuildServiceProvider()
    {
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }

    protected T GetService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}