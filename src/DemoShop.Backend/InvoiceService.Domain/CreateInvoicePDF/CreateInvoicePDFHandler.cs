using Common.CommandHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceService.Domain.CreateInvoicePDF
{
    public record CreateInvoicePDFCommand(Guid orderId);
    public class CreateInvoicePDFHandler : ICommandHandler<CreateInvoicePDFCommand>
    {
        public Task HandleAsync(CreateInvoicePDFCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
