using Microsoft.Extensions.DependencyInjection;

namespace NightTasker.UserHub.IntegrationTests.Framework;

public record ServiceForRegister(
    Type Type, Func<IServiceProvider, object>? Factory = null, ServiceLifetime? Lifetime = null);