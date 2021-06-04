namespace Checkout.Application.UseCases.SetPaymentConfirmed;

public interface ISetPaymentConfirmedUseCase
{
    Task ExecuteAsync(PaymentConfirmed paymentConfirmed, CancellationToken cancellationToken = default);
}
