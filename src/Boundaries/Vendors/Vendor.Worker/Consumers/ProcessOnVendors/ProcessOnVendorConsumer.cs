using Microsoft.Extensions.Logging;

namespace Vendor.Worker.Consumers.ProcessOnVendors;

public class ProcessOnVendorConsumer : IConsumer<ProcessOnVendor>
{
    private readonly ILogger<ProcessOnVendorConsumer> _logger;

    public ProcessOnVendorConsumer(ILogger<ProcessOnVendorConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessOnVendor> context)
    {
        _logger.LogInformation($"Pedido {context.Message.CorrelationId} integrado no parceiro");

        VendorProcessed @event = new()
        {
            CorrelationId = context.Message.CorrelationId,
            OrderId = context.Message.OrderId
        };

        await context.Publish(@event);
    }
}
