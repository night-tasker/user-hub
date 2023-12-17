using Mapster;
using MapsterMapper;

namespace NightTasker.UserHub.Presentation.WebApi.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddMapper();
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
}