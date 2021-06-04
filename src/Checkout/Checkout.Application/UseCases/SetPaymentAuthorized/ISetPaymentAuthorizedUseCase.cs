namespace Checkout.Application.UseCases.SetPaymentAuthorized;

public interface ISetPaymentAuthorizedUseCase
{
    Task ExecuteAsync(PaymentAuthorized paymentAuthorized, CancellationToken cancellationToken = default);
}
