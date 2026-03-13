using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrderService.Api.HealthChecks
{
    public class AvailabiltyHealthCheck : IHealthCheck
    {
        public static string Name => nameof(AvailabiltyHealthCheck);
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The service is available."));
        }
    }
}
