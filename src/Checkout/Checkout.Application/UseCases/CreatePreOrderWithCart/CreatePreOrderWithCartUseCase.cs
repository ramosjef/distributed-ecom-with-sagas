using Checkout.Domain.Orders;

using MassTransit;

namespace Checkout.Application.UseCases.CreatePreOrderWithCart;

internal sealed class CreatePreOrderWithCartUseCase : ICreatePreOrderWithCartUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBus _bus;

    public CreatePreOrderWithCartUseCase(IOrderRepository orderRepository, IBus bus)
    {
        _orderRepository = orderRepository;
        _bus = bus;
    }

    public async Task<long> ExecuteAsync(CreatePreOrderWithCartRequest request, CancellationToken cancellationToken)
    {
        Order order = Order.InitPreOrder();

        OrderId orderId = await _orderRepository.CreateAsync(order, cancellationToken);

        await _bus.Publish(new PreOrderCreated()
        {
            OrderId = orderId,
            CorrelationId = request.CorrelationId,
            CreatedDateTime = DateTime.UtcNow
        },
        cancellationToken);

        return orderId;
    }
}
