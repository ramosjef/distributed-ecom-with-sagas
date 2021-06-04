using Checkout.Domain.Orders;

namespace Checkout.Application.UseCases.SetVendorProcessed;

internal class SetVendorProcessedUseCase : ISetVendorProcessedUseCase
{
    private readonly IOrderRepository _orderRepository;

    public SetVendorProcessedUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(VendorProcessed vendorProcessed, CancellationToken cancellationToken = default)
    {
        Order order = await _orderRepository.GetAsync(vendorProcessed.OrderId, cancellationToken);

        order.SetVendorProcessed(vendorProcessed);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
