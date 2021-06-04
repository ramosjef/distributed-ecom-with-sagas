namespace Checkout.Domain.Orders;

public enum OrderStatus
{
    None,
    PreOrder,
    PaymentAuthorized,
    CreatedOnVendor,
    PaymentConfirmed,
    SecondFactorRequired,
    OnboardingRequired,
}
