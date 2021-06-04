using Checkout.Domain.Orders;

namespace Checkout.Application.UseCases.SetPaymentAuthorized;

internal class SetPaymentAuthorizedUseCase : ISetPaymentAuthorizedUseCase
{
    private readonly IOrderRepository _orderRepository;

    public SetPaymentAuthorizedUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(PaymentAuthorized paymentAuthorized, CancellationToken cancellationToken = default)
    {
        Order order = await _orderRepository.GetAsync(paymentAuthorized.OrderId, cancellationToken);

        order.SetPaymentAuthorized(paymentAuthorized);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
