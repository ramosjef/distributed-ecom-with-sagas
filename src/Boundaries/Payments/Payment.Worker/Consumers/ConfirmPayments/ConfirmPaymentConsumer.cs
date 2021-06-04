using Microsoft.Extensions.Logging;

namespace Payment.Worker.Consumers.ConfirmPayments;

public class ConfirmPaymentConsumer : IConsumer<ConfirmPayment>
{
    private readonly ILogger<ConfirmPaymentConsumer> _logger;

    public ConfirmPaymentConsumer(ILogger<ConfirmPaymentConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ConfirmPayment> context)
    {
        _logger?.LogInformation($"{context.Message.CorrelationId} => Pagamento processado");

        PaymentConfirmed @event = new()
        {
            CorrelationId = context.Message.CorrelationId,
            OrderId = context.Message.OrderId
        };

        await context.Publish(@event);
    }
}
