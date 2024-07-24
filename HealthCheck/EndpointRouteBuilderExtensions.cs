using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace OpenTelemetryTests.HealthCheck
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapCustomHealthCheck(
                this IEndpointRouteBuilder endpoints,
                string readyPattern = "/health/ready",
                string livenessPattern = "/health/liveness")
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            endpoints.MapHealthChecks(readyPattern, new HealthCheckOptions()
            {
                Predicate = (check) => true,
                AllowCachingResponses = false,
            });
            endpoints.MapHealthChecks(livenessPattern, new HealthCheckOptions()
            {
                Predicate = (check) => true,
                AllowCachingResponses = false,
            });

            return endpoints;
        }
    }
}
