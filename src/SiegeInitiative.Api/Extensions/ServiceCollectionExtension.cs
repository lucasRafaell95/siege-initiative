using Microsoft.OpenApi.Models;

namespace SiegeInitiative.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRouting();

        services.AddSwagger();

        services.AddHealthCheck(configuration);

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("SiegeInitiative"))
            .AddRedis(configuration.GetValue<string>("DistributedCache:ConnectionString"));

        return services;
    }

    private static IServiceCollection ConfigureRouting(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(_ => _.LowercaseUrls = true);

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(_ =>
        {
            _.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Rainbow Six Siege Initiative API",
                Description = "The open data API for rainbow six siege game",
                Contact = new OpenApiContact
                {
                    Name = "Lucas Santos",
                    Email = "lucas.rafaellns@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/lucas-rnsantos/")
                }
            });
        });

        return services;
    }
}