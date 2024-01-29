using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;

namespace NightTasker.UserHub.Presentation.WebApi.IntegrationTests;

public abstract class BaseIntegrationTests
{
    protected readonly TestWebApplicationFactory WebApplicationFactory;
    protected readonly HttpClient HttpClient;

    protected BaseIntegrationTests(IReadOnlyCollection<ServiceForRegister>? mockServices = null, bool mockAuthorization = false)
    {
        var clientOptions = new WebApplicationFactoryClientOptions();
        var testNpgSql = new TestNpgSql();
        WebApplicationFactory = new TestWebApplicationFactory(testNpgSql, mockServices, mockAuthorization);
        HttpClient = WebApplicationFactory.CreateClient(clientOptions);
    }
    
    protected ApplicationDbContext GetDbContextService()
    {
        var scope = WebApplicationFactory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public async ValueTask DisposeAsync()
    {
        await WebApplicationFactory.DisposeAsync();
        HttpClient.Dispose();
    }
}