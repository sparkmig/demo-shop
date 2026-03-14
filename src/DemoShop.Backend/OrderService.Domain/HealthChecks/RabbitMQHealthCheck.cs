using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace OrderService.Domain.HealthChecks
{
    /// <summary>
    /// We inject the serviceprovider, as the connection check when the factoru
    /// </summary>
    /// <param name="serviceProvider"></param>
    public class RabbitMQHealthCheck(IConnectionFactory connectionFactory) : IHealthCheck
    {
        public static string Name => nameof(RabbitMQHealthCheck);
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
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
