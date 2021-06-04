using System.Diagnostics.CodeAnalysis;

using Checkout.Domain.Orders;

using MongoDB.Driver;

namespace Checkout.Infrastructure.DataAccess.Mongo;

[ExcludeFromCodeCoverage]
public sealed class MongoDbContext : IMongoContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoConfiguration config)
    {
        MongoClientSettings settings = MongoClientSettings.FromConnectionString(
            $"mongodb://{config.User}:{config.Password}@{config.Server}:{config.Port}");

        settings.ClusterConfigurator = cb => { };

        MongoClient = new MongoClient(settings);

        _database = MongoClient.GetDatabase(config.DatabaseName);
    }

    public IMongoClient MongoClient { get; private set; }

    public IMongoCollection<Order> Orders =>
        _database.GetCollection<Order>(typeof(Order).Name);
}
