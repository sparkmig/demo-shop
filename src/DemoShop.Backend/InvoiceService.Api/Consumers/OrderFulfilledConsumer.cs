using Common.CommandHandler;
using Common.IntegrationEvents;
using InvoiceService.Domain.CreateInvoicePDF;
using MassTransit;

namespace InvoiceService.Api.Consumers
{
    public class OrderFulfilledConsumer(Dispatcher dispatcher) : IConsumer<OrderFulfilledEvent>
    {
        public async Task Consume(ConsumeContext<OrderFulfilledEvent> context)
        {
            var orderId = context.Message.OrderId;
            await dispatcher.DispatchAsync(new CreateInvoicePDFCommand(orderId));
            // Handle the order fulfilled event (e.g., update invoice status)
        }
    }
}
