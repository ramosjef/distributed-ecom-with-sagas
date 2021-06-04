using Checkout.Domain.Orders;

namespace Checkout.Infrastructure.DataAccess.InMemory.Repositories;

public class OrderRepository : IOrderRepository
{
    public List<Order> Orders;

    public OrderRepository()
    {
        Orders = new List<Order>();
    }

    public Task<OrderId> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = order.Id > 0 ? order.Id : new Random(100000).NextInt64();
        Orders.Add(order);

        return Task.FromResult(order.Id);
    }

    public Task<Order> GetAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        Order result = Orders.FirstOrDefault(x => x.Id.Equals(id));

        return Task.FromResult(result!);
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        if (Orders.FirstOrDefault(x => x.Id.Equals(order.Id)) is Order found)
            Orders.Remove(found);

        Orders.Add(order);

        return Task.CompletedTask;
    }
}
