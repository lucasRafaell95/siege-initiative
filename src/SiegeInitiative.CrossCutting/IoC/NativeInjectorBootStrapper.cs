using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiegeInitiative.Infrastructure.Persistence.Extensions;

namespace SiegeInitiative.CrossCutting.IoC;

public static class NativeInjectorBootStrapper
{
    public static IServiceCollection RegisterAplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddPersistenceDependencies(configuration);

        return services;
    }
}