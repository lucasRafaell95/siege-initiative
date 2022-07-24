using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace SiegeInitiative.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health");

        app.UseHealthChecks("/health/details", new HealthCheckOptions()
        {
            ResponseWriter = (httpContext, result) =>
            {
                httpContext.Response.ContentType = "application/json; charset=utf-8";

                var json = new
                {
                    Status = result.Status,
                    Duration = result.TotalDuration,
                    Resource = new
                    {
                        AssemblyName = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Name,
                        AssemblyVersion = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly()?.Location)?.Version?.ToString(),
                        Resource = Environment.MachineName.Remove(0, Environment.MachineName.Length - 5),
                    },
                    Services = result.Entries.Select(_ =>
                        new
                        {
                            Service = _.Key,
                            Status = _.Value.Status.ToString(),
                            Duration = _.Value.Duration,
                            Description = _.Value.Description,
                            Data = _.Value.Data,
                            Exception = _.Value.Exception?.Message
                        })
                };

                return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(json, new StringEnumConverter()));
            }
        });

        return app;
    }
}