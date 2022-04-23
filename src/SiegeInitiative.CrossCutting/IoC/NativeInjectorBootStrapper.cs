using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiegeInitiative.Application.Extensions;
using SiegeInitiative.Infrastructure.Persistence.Extensions;

namespace SiegeInitiative.CrossCutting.IoC;

public static class NativeInjectorBootStrapper
{
    public static IServiceCollection RegisterAplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Application
        services.AddApplicationDependecies(configuration);

        // Persistence
        services.AddPersistenceDependencies(configuration);

        return services;
    }
}