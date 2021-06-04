using Checkout.Domain.Orders;

namespace Checkout.Application.UseCases.SetPaymentConfirmed;

internal class SetPaymentConfirmedUseCase : ISetPaymentConfirmedUseCase
{
    private readonly IOrderRepository _orderRepository;

    public SetPaymentConfirmedUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(PaymentConfirmed paymentConfirmed, CancellationToken cancellationToken = default)
    {
        Order order = await _orderRepository.GetAsync(paymentConfirmed.OrderId, cancellationToken);

        order.SetPaymentConfirmed(paymentConfirmed);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
