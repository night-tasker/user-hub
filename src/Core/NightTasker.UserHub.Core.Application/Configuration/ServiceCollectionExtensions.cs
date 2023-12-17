using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Services.Implementations;

namespace NightTasker.UserHub.Core.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });
        
        return services;
    }
}