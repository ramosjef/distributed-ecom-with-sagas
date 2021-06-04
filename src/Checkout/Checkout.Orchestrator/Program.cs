using System;
using System.Reflection;
using System.Threading.Tasks;

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Checkout.Orchestrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();

            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureServices((hostContext, svc) =>
                   {
                       svc.AddLogging(cfg => cfg.AddConsole());

                       svc.AddMassTransit(cfg =>
                       {
                           cfg.AddSagaStateMachines(Assembly.GetExecutingAssembly());
                           cfg.SetMongoDbSagaRepositoryProvider(cfg =>
                           {
                               cfg.Connection = hostContext.Configuration.GetConnectionString("MongoDb");
                               cfg.DatabaseName = hostContext.Configuration.GetValue<string>("MongoDb:DatabaseName");
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
}
