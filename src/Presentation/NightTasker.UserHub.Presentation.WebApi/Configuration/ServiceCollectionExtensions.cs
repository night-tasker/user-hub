using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
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
    public static IServiceCollection ConfigureControllers(
        this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }
    
    public static IServiceCollection RegisterApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureAuthentication(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IApplicationDataAccessor, ApplicationDataAccessor>();
        services.AddValidation();
        return services;
    }

    private static void ConfigureAuthentication(this IServiceCollection services,
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
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableDataAnnotationsValidation = true;
        });
    }
}