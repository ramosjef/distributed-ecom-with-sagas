using System.Diagnostics.CodeAnalysis;

using Checkout.Domain.Orders;
using Checkout.Infrastructure.DataAccess.Mongo.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Infrastructure.DataAccess.Mongo;

[ExcludeFromCodeCoverage]
public static class Extensions
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services, IConfiguration config)
    {
        MongoConfiguration mongo = new();
        config.GetSection("MongoDB").Bind(mongo);

        services.AddSingleton<IMongoContext>(x => new MongoDbContext(mongo));
        services.AddSingleton<IOrderRepository, OrderRepository>();

        return services;
    }
}
