using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = Host
    .CreateDefaultBuilder(args)
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
    })
    .Build();

await host.RunAsync();

Console.ReadKey();

