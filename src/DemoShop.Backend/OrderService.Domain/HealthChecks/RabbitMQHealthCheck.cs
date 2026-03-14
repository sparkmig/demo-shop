using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace OrderService.Domain.HealthChecks
{
    public class RabbitMQHealthCheck(IConnection connection) : IHealthCheck
    {
        public static string Name => nameof(RabbitMQHealthCheck);
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
                if (channel == null)
                {
                    return HealthCheckResult.Unhealthy("Failed to create RabbitMQ channel");
                }
                await connection.CloseAsync(cancellationToken);
                return HealthCheckResult.Healthy("RabbitMQ connection OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("RabbitMQ connection failed", ex);
            }
        }
    }
}
