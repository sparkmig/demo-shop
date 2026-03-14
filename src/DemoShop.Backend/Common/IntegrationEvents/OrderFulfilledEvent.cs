using System;
using System.Collections.Generic;
using System.Text;

namespace Common.IntegrationEvents
{
    public record OrderFulfilledEvent(Guid OrderId);
}
