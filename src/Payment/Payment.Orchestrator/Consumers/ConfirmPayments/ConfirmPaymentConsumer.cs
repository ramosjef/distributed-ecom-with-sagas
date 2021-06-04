using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MassTransit;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Payment.Orchestrator.Consumers.ConfirmPayments
{
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

            PaymentConfirmed @event = new() { CorrelationId = context.Message.CorrelationId };
            await context.Publish(@event);
        }
    }
}
