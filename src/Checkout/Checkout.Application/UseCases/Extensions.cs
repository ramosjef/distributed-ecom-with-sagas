using Checkout.Application.UseCases.CreatePreOrderWithCart;
using Checkout.Application.UseCases.SetPaymentAuthorized;
using Checkout.Application.UseCases.SetPaymentConfirmed;
using Checkout.Application.UseCases.SetVendorProcessed;

using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Application.UseCases;

public static class Extensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreatePreOrderWithCartUseCase, CreatePreOrderWithCartUseCase>();
        services.AddScoped<ISetPaymentAuthorizedUseCase, SetPaymentAuthorizedUseCase>();
        services.AddScoped<ISetVendorProcessedUseCase, SetVendorProcessedUseCase>();
        services.AddScoped<ISetPaymentConfirmedUseCase, SetPaymentConfirmedUseCase>();

        return services;
    }
}
