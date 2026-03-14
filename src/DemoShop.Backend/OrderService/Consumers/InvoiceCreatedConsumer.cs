using Common.IntegrationEvents;
using MassTransit;

namespace OrderService.Api.Consumers
{
    public class InvoiceCreatedConsumer : IConsumer<InvoiceCreatedEvent>
    {
        public Task Consume(ConsumeContext<InvoiceCreatedEvent> context)
        {
            //Set order status to "Invoiced" or perform any necessary actions based on the invoice creation event
            var orderId = context.Message.OrderId;

            return Task.CompletedTask;
        }
    }
}
