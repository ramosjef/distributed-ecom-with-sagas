namespace Checkout.Domain.Orders;

public interface IOrderRepository
{
    public Task<Order> GetAsync(OrderId id, CancellationToken cancellationToken = default);
    public Task<OrderId> CreateAsync(Order order, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}
