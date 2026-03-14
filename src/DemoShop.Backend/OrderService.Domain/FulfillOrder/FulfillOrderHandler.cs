using Common.CommandHandler;
using Common.IntegrationEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.FulfillOrder
{
    public record FulfillOrderCommand(Guid OrderId);
    
    public class FulfillOrderHandler(IBus bus) : ICommandHandler<FulfillOrderCommand>
    {
        public async Task HandleAsync(FulfillOrderCommand command)
        {
            // Do order stuff;

            // Publish an event that the order was fulfilled
            await bus.Publish(new OrderFulfilledEvent(command.OrderId));
        }
    }
}
