using Microsoft.Extensions.Logging;

namespace Payment.Worker.Consumers.AuthorizePayments;

public class AuthorizePaymentConsumer : IConsumer<AuthorizePayment>
{
    private readonly ILogger<AuthorizePaymentConsumer> _logger;

    public AuthorizePaymentConsumer(ILogger<AuthorizePaymentConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuthorizePayment> context)
    {
        _logger?.LogInformation($"{context.Message.CorrelationId} => Pagamento processado");

        PaymentAuthorized @event = new()
        {
            OrderId = context.Message.OrderId,
            CorrelationId = context.Message.CorrelationId
        };

        await context.Publish(@event);
    }
}
