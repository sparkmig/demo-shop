using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace OrderService.Domain.Configure
{
    public static class RabbitMQ
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var config = sp.GetService<IConfiguration>();

                var factory = new ConnectionFactory
                {
                    HostName = config["RabbitMQ:HostName"] ?? throw new MissingEnvironmentVariableException("RabbitMQ:HostName"),
                    UserName = config["RabbitMQ:Username"] ?? throw new MissingEnvironmentVariableException("RabbitMQ:Username"),
                    Password = config["RabbitMQ:Password"] ?? throw new MissingEnvironmentVariableException("RabbitMQ:Password")
                };

                return factory;
            });
        }
    }
}
