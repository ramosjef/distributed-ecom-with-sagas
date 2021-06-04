using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MassTransit;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Payment.Orchestrator.Consumers.CancelAuthorizations
{
    public class CancelAuthorizationConsumer : IConsumer<CancelAuthorization>
    {
        private readonly ILogger<CancelAuthorizationConsumer> _logger;

        public CancelAuthorizationConsumer(ILogger<CancelAuthorizationConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CancelAuthorization> context)
        {
            _logger?.LogInformation($"{context.Message.CorrelationId} => Autorização cancelada");

            AuthorizationCanceled @event = new() { CorrelationId = context.Message.CorrelationId };
            await context.Publish(@event);
        }
    }
}
