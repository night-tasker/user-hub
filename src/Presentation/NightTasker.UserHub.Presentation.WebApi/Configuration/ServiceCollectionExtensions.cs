﻿using System.Text;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.Common.Core.Identity.Implementations;
using NightTasker.UserHub.Infrastructure.Persistence.Contracts;
using NightTasker.UserHub.Presentation.WebApi.Implementations;
using NightTasker.UserHub.Presentation.WebApi.Settings;

namespace NightTasker.UserHub.Presentation.WebApi.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMapper();
        services.ConfigureAuthentication(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IApplicationDbAccessor, ApplicationDbAccessor>();
        return services;
    }
    
    /// <summary>
    /// Добавить Маппер.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(
            typeof(ServiceCollectionExtensions).Assembly,
            typeof(Core.Application.Configuration.ServiceCollectionExtensions).Assembly);
        typeAdapterConfig.RequireExplicitMapping = true;
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
        return services;
    }

    private static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var identitySettingsSection = configuration.GetSection(nameof(IdentitySettings));
        var identitySettings = identitySettingsSection.Get<IdentitySettings>()!;
        services.Configure<IdentitySettings>(identitySettingsSection);
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var secretKey = Encoding.UTF8.GetBytes(identitySettings.SecretKey);
            var encryptionKey = Encoding.UTF8.GetBytes(identitySettings.EncryptKey);

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true,
                ValidAudience = identitySettings.Audience,

                ValidateIssuer = true,
                ValidIssuer = identitySettings.Issuer,

                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey),
            };

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
        });

        return services;
    }
}