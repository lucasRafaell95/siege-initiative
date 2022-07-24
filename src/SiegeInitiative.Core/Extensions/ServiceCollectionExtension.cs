using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiegeInitiative.Core.Options;
using SiegeInitiative.Core.Options.Base;

namespace SiegeInitiative.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreDependecies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IRedisCacheOptions>(_ => configuration.GetSection("DistributedCache").Get<RedisCacheOptions>());

        return services;
    }
}