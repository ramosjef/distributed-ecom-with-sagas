using System.Threading.Tasks;

using MassTransit;
using Microsoft.Extensions.Logging;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Vendor.Orchestrator.Consumers.CancelVendorOrders
{
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

            VendorOrderCanceled @event = new() { CorrelationId = context.Message.CorrelationId };
            await context.Publish(@event);
        }
    }
}
