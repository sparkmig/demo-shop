using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CommandHandler
{
    public static class ConfigureCommandHandlers
    {
        public static void AddDispatcher(this IServiceCollection services)
        {
            services.AddScoped<Dispatcher>();
        }
    }
}
