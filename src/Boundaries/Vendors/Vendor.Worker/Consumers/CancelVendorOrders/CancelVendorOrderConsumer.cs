using Microsoft.Extensions.Logging;

namespace Vendor.Worker.Consumers.CancelVendorOrders;

public class CancelVendorOrderConsumer : IConsumer<CancelVendorOrder>
{
    private readonly ILogger<CancelVendorOrder> _logger;

    public CancelVendorOrderConsumer(ILogger<CancelVendorOrder> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CancelVendorOrder> context)
    {
        _logger.LogInformation($"Pedido {context.Message.CorrelationId} cancelado no parceiro");

        VendorOrderCanceled @event = new()
        {
            CorrelationId = context.Message.CorrelationId,
            OrderId = context.Message.OrderId
        };

        await context.Publish(@event);
    }
}
