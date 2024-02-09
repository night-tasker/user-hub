using Microsoft.Extensions.DependencyInjection;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUserInvites.Services;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.OrganizationUsers.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Implementations;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Contracts;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Implementations;

namespace NightTasker.UserHub.Core.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserImageService, UserImageService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IOrganizationUserService, OrganizationUserService>();
        services.AddScoped<IOrganizationUserInviteService, OrganizationUserInviteService>();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });
        
        return services;
    }
}