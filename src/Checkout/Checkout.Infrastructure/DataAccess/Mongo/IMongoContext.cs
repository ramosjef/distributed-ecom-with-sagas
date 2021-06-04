using Checkout.Domain.Orders;

using MongoDB.Driver;

namespace Checkout.Infrastructure.DataAccess.Mongo;

public interface IMongoContext
{
    IMongoClient MongoClient { get; }
    IMongoCollection<Order> Orders { get; }
}
