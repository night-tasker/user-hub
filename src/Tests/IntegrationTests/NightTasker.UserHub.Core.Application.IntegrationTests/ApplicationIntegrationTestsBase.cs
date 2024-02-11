using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;
using Xunit;

namespace NightTasker.UserHub.Core.Application.IntegrationTests;

[Collection("NpgSqlTestCollection")]
public abstract class ApplicationIntegrationTestsBase
{
    private readonly IServiceCollection _serviceCollection;
    private IServiceProvider _serviceProvider = null!;
    private readonly TestNpgSql _testNpgSqlFixture;

    protected ApplicationIntegrationTestsBase(TestNpgSql testNpgSql)
    {
        _testNpgSqlFixture = testNpgSql;
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddDbContext<ApplicationDbContext>(
            (_, option) => option.UseNpgsql($"{_testNpgSqlFixture.NpgSqlContainer.GetConnectionString()};Include Error Detail=true"));
    }

    protected void RegisterService(ServiceForRegister serviceForRegister)
    {
        switch (serviceForRegister)
        {
            case { Lifetime: ServiceLifetime.Singleton }:
                if (serviceForRegister.Factory == null)
                    _serviceCollection.AddSingleton(serviceForRegister.Type);
                else
                    _serviceCollection.AddSingleton(serviceForRegister.Type, serviceForRegister.Factory);
                break;
            case { Lifetime: ServiceLifetime.Scoped }:
                if (serviceForRegister.Factory == null)
                    _serviceCollection.AddScoped(serviceForRegister.Type);
                else
                    _serviceCollection.AddScoped(serviceForRegister.Type, serviceForRegister.Factory);
                break;
            case { Lifetime: ServiceLifetime.Transient }:
                if (serviceForRegister.Factory == null)
                    _serviceCollection.AddTransient(serviceForRegister.Type);
                else
                    _serviceCollection.AddTransient(serviceForRegister.Type, serviceForRegister.Factory);
                break;
            case { Lifetime: null }:
                if (serviceForRegister.Factory == null)
                    _serviceCollection.AddScoped(serviceForRegister.Type);
                else
                    _serviceCollection.AddScoped(serviceForRegister.Type, serviceForRegister.Factory);
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

    protected IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }

    protected AsyncServiceScope CreateAsyncScope()
    {
        return _serviceProvider.CreateAsyncScope();
    }

    protected void DropDatabase()
    {
        _testNpgSqlFixture.DropDatabase();
    }

    protected void PrepareDatabase()
    {
        _testNpgSqlFixture.DropDatabase();
        GetService<ApplicationDbContext>().Database.Migrate();
    }
}

[CollectionDefinition("NpgSqlTestCollection")]
public class NpgSqlTestCollection : ICollectionFixture<TestNpgSql>;