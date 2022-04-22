using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiegeInitiative.Domain.Persistence.Repositories.Base;
using SiegeInitiative.Infrastructure.Persistence.Core;
using SiegeInitiative.Infrastructure.Persistence.Repositories;

namespace SiegeInitiative.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        => services.AddRepositoriesDependencies(configuration);

    private static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfContext(configuration);

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork>(_ => _.GetRequiredService<SiegeDbContext>());

        return services;
    }

    private static IServiceCollection AddEfContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SiegeDbContext>(_ =>
        {
            _.UseSqlServer(configuration.GetConnectionString("SiegeInitiative"));
        });

        return services;
    }
}