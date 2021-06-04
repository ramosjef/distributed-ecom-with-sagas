using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MassTransit;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Payment.Orchestrator.Consumers.AuthorizePayments
{
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

            PaymentAuthorized @event = new() { CorrelationId = context.Message.CorrelationId };
            await context.Publish(@event);
        }
    }
}
