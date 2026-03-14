using Common.CommandHandler;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.FulfillOrder;

namespace OrderService.Api.Controllers
{
    [Route("orders")]
    public class OrderController(Dispatcher dispatcher) : Controller
    {
        [HttpPost("{orderId:guid}/fulfill")]
        public async Task<IActionResult> FulfillOrder(Guid orderId)
        {
            await dispatcher.DispatchAsync(new FulfillOrderCommand(orderId));

            return Ok();
        }
    }
}
