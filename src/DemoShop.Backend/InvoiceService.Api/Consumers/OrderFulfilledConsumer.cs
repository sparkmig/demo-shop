using Common.IntegrationEvents;
using MassTransit;

namespace InvoiceService.Api.Consumers
{
    public class OrderFulfilledConsumer : IConsumer<OrderFulfilledEvent>
    {
        public async Task Consume(ConsumeContext<OrderFulfilledEvent> context)
        {
            var orderId = context.Message.OrderId;

            // Handle the order fulfilled event (e.g., update invoice status)
        }
    }
}
