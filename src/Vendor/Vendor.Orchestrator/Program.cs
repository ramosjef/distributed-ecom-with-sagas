using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Vendor.Orchestrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();

            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddLogging(cfg => cfg.AddConsole());
                       services.AddMassTransit(x =>
                       {
                           x.AddConsumers(typeof(Program).Assembly);
                           x.SetKebabCaseEndpointNameFormatter();
                           x.UsingRabbitMq((context, cfg) =>
                           {
                               cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));
                               cfg.ConfigureEndpoints(context);
                           });
                       });
                   });
    }
}
