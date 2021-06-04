using System.Diagnostics.CodeAnalysis;

using Checkout.Domain.Orders;

using MongoDB.Driver;

namespace Checkout.Infrastructure.DataAccess.Mongo.Repositories;

[ExcludeFromCodeCoverage]
public class OrderRepository : IOrderRepository
{
    private readonly IMongoContext _mongoContext;

    public OrderRepository(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<OrderId> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = Random.Shared.NextInt64();

        await _mongoContext.Orders.InsertOneAsync(order, cancellationToken: cancellationToken);

        return order.Id;
    }

    public async Task<Order> GetAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        var result = await _mongoContext.Orders.FindAsync(x
            => x.Id == id, cancellationToken: cancellationToken);

        return result.FirstOrDefault(cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _mongoContext.Orders.FindOneAndReplaceAsync(x
            => x.Id.Equals(order.Id), order, cancellationToken: cancellationToken);
    }
}
