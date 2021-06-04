using System.Net;

using Checkout.Application.UseCases.CreatePreOrderWithCart;

using CorrelationId.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(string))]
    public async Task<IActionResult> CreatePrePreOrder([FromServices] ICorrelationContextAccessor correlationContext,
                                                       [FromServices] ICreatePreOrderWithCartUseCase createPreOrderUseCase)
    {
        long orderId = await createPreOrderUseCase.ExecuteAsync(
            new CreatePreOrderWithCartRequest(
                Guid.Parse(correlationContext.CorrelationContext.CorrelationId)));

        return Accepted(new
        {
            OrderId = orderId,
            correlationContext.CorrelationContext.CorrelationId
        });
    }
}
