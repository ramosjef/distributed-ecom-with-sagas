using Microsoft.Extensions.Logging;

namespace Payment.Worker.Consumers.CancelAuthorizations;

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

        AuthorizationCanceled @event = new()
        {
            CorrelationId = context.Message.CorrelationId,
            OrderId = context.Message.OrderId
        };

        await context.Publish(@event);
    }
}
