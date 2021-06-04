using MassTransit;

using Microsoft.OpenApi.Models;

namespace Checkout.Api.Common;

public static class Extensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout.Api", Version = "v1" });
        });

        return services;
    }

    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"));
            });
        });

        return services;
    }
}
