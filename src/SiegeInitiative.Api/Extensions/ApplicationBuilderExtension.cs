using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                httpContext.Response.ContentType = "application/json";

                var json = new JObject(
                    new JProperty("status", result.Status.ToString()),
                    new JProperty("results", new JObject(result.Entries.Select(pair =>
                        new JProperty(pair.Key, new JObject(
                            new JProperty("status", pair.Value.Status.ToString()),
                            new JProperty("description", pair.Value.Description),
                            new JProperty("data", new JObject(pair.Value.Data.Select(
                                p => new JProperty(p.Key, p.Value))))))))));
                return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
            }
        });

        return app;
    }
}