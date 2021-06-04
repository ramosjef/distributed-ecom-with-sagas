using System.Diagnostics.CodeAnalysis;

using Checkout.Application.UseCases;
using Checkout.Infrastructure.DataAccess.Mongo;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Checkout.Orchestrator;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        await host.RunAsync();

        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, svc) =>
               {
                   svc.AddLogging(cfg => cfg.AddConsole())
                      .AddMongoRepositories(hostContext.Configuration)
                      .AddUseCases();

                   svc.AddMassTransit(cfg =>
                   {
                       cfg.AddSagaStateMachines(Assembly.GetExecutingAssembly());
                       cfg.SetMongoDbSagaRepositoryProvider(cfg =>
                       {
                           MongoConfiguration mongo = new();
                           hostContext.Configuration.GetSection("MongoDB").Bind(mongo);
                           cfg.Connection = $"mongodb://{mongo.User}:{mongo.Password}@{mongo.Server}:{mongo.Port}";
                           cfg.DatabaseName = mongo.DatabaseName;
                       });

                       cfg.UsingRabbitMq((context, cfg) =>
                       {
                           cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                           cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
                           cfg.UseInMemoryOutbox();
                           cfg.UseMessageRetry(r => r.Immediate(30));
                       });
                   });
               });
}
