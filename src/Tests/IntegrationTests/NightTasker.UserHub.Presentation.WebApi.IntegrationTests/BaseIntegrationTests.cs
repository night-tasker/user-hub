using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;

namespace NightTasker.UserHub.Presentation.WebApi.IntegrationTests;

public abstract class BaseIntegrationTests
{
    private readonly TestWebApplicationFactory _webApplicationFactory;
    protected readonly HttpClient HttpClient;

    protected BaseIntegrationTests(IReadOnlyCollection<ServiceForRegister>? mockServices = null, bool mockAuthorization = false)
    {
        var clientOptions = new WebApplicationFactoryClientOptions();
        var testNpgSql = new TestNpgSql();
        _webApplicationFactory = new TestWebApplicationFactory(testNpgSql, mockServices, mockAuthorization);
        HttpClient = _webApplicationFactory.CreateClient(clientOptions);
    }
    
    protected ApplicationDbContext GetDbContextService()
    {
        var scope = _webApplicationFactory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}