﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Grpc.StorageFiles;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Services;
using NightTasker.UserHub.Infrastructure.Grpc.Implementations.Client.StorageFile;
using NightTasker.UserHub.Infrastructure.Grpc.Settings;

namespace NightTasker.UserHub.Infrastructure.Grpc.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGrpcServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGrpcClients(configuration);
        services.AddServices();
        return services;
    }

    private static void AddGrpcClients(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStorageFileGrpcClient(configuration);
    }

    private static void AddStorageFileGrpcClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var storageGrpcSettingsSection = configuration.GetSection(nameof(StorageGrpcSettings));
        var storageGrpcSettings = new StorageGrpcSettings();
        storageGrpcSettingsSection.Bind(storageGrpcSettings);
        services.Configure<StorageGrpcSettings>(storageGrpcSettingsSection);
        
        services.AddGrpcClient<StorageFile.StorageFileClient>(configure =>
        {
            configure.Address = new Uri($"{storageGrpcSettings.Host}:{storageGrpcSettings.Port}");
        });
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageFileService, StorageFileService>();
    }
}