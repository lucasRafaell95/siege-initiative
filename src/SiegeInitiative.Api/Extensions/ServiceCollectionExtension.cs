namespace SiegeInitiative.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwagger();

        services.AddHealthCheck(configuration);

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen();

    private static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("SiegeInitiative"));

        return services;
    }
}