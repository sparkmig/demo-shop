using System;
using System.Collections.Generic;
using System.Text;

namespace Common.IntegrationEvents
{
    public record InvoiceCreatedEvent(Guid OrderId);
}
