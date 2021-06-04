namespace Checkout.Application.UseCases.CreatePreOrderWithCart;

public class CreatePreOrderWithCartRequest
{
    public CreatePreOrderWithCartRequest(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
}
