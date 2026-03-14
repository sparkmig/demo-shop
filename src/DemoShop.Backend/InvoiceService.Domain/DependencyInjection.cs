using Common.CommandHandler;
using InvoiceService.Domain.CreateInvoicePDF;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Domain
{
    public static class DependencyInjection
    {
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<CreateInvoicePDFCommand>, CreateInvoicePDFHandler>();
        }
    }
}
