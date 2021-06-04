namespace Checkout.Application.UseCases.CreatePreOrderWithCart;

public interface ICreatePreOrderWithCartUseCase
{
    Task<long> ExecuteAsync(CreatePreOrderWithCartRequest request, CancellationToken cancellationToken = default);
}
