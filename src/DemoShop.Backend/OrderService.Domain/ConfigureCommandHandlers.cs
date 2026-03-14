using Common.CommandHandler;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.FulfillOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain
{
    public static class ConfigureCommandHandlers
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<FulfillOrderCommand>, FulfillOrderHandler>();
        }
    }
}
