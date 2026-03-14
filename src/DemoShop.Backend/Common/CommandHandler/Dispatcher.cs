using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CommandHandler
{
    public class Dispatcher(IServiceProvider serviceProvider)
    {
        public async Task DispatchAsync<T>(T command)
        {
            if (serviceProvider.GetService(typeof(ICommandHandler<T>)) is ICommandHandler<T> handler)
            {
                await handler.HandleAsync(command);
            }
        }

        public async Task<TResult>? DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            if (serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) is IQueryHandler<TQuery, TResult> handler)
            {
                return await handler.GetAsync(query);
            }

            return default;
        }
    }
}
