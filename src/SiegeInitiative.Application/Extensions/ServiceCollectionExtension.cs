using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiegeInitiative.Application.Caching.Base;
using SiegeInitiative.Application.Options;
using SiegeInitiative.Application.Options.Base;

namespace SiegeInitiative.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationDependecies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IRedisCacheOptions>(_ => configuration.GetSection("DistributedCache").Get<RedisCacheOptions>());

        services.AddCacheServices();

        return services;
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRedisCacheService<,>), typeof(RedisCacheService<,>));

        return services;
    }
}