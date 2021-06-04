using System;
using System.Threading.Tasks;

using MassTransit;
using Microsoft.AspNetCore.Mvc;

using Checkout.Api.Dtos;
using Shared.Messages.Events;

namespace Checkout.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IBus _bus;

        public OrderController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrePreOrder(CreatePreOrderDto request)
        {
            PreOrderCreated @event = new() { CorrelationId = Guid.NewGuid(), CreatedDateTime = DateTime.UtcNow };
            await _bus.Publish(@event);

            return Accepted(@event.CorrelationId);
        }
    }
}
